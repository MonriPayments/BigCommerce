using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Services.Checkout.GetIFrameValues;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
using BigCommerceApi.Domain.Services.WSPayForm;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Common.Contracts;
using ILogger = WebStudio.Logging.Abstractions.ILogger;
using PaymentGatewayEnum = BigCommerceApi.Domain.Model.Stores.PaymentGateway;

namespace BigCommerceApi.Domain.Services.PaymentGateways.WSPay
{
    public sealed class WSPayGateway : IPaymentGateway
    {
        private readonly IFormWSPay _formWsPay;
        private readonly FormWSPayConfiguration _formConfiguration;
        private readonly ILogger _logger;

        public WSPayGateway(IFormWSPay formWsPay, FormWSPayConfiguration formConfiguration, ILogger logger)
        {
            Argument.IsNotNull(formWsPay, nameof(formWsPay));
            Argument.IsNotNull(formConfiguration, nameof(formConfiguration));
            Argument.IsNotNull(logger, nameof(logger));

            _formWsPay = formWsPay;
            _formConfiguration = formConfiguration;
            _logger = logger;
        }

        public PaymentGatewayEnum Gateway => PaymentGatewayEnum.WSPay;

        public string PaymentMethodDescription => "WSPay by Monri";

        // Per-merchant iframe response target overrides. WSPay defaults to "TOP";
        // a small allowlist of merchants need "self" so the postback stays inside the iframe.
        // When per-account JSON config exists on Shops, move these in there.
        private static readonly HashSet<string> _selfTargetShopIds = new(System.StringComparer.OrdinalIgnoreCase)
        {
            "SKOLSKA"
        };

        private static string ResolveIframeResponseTarget(BigCommerceApi.Domain.Model.Shops.Shops shop)
        {
            return shop.ShopID != null && _selfTargetShopIds.Contains(shop.ShopID) ? "self" : "TOP";
        }

        public async Task<string?> CreateRedirectFormUrlAsync(GatewayCheckoutContext context, CancellationToken cancellationToken)
        {
            var shop = context.Shops.FirstOrDefault(s => s.IsTokenShopId == false);
            if (shop == null)
                return null;

            var formBaseUrl = context.StoreConfiguration.StoreStatus == StoreStatus.Production
                ? _formConfiguration.BaseUrlProd
                : _formConfiguration.BaseUrlTest;

            var formResponse = await _formWsPay.CreateFormTransactionAsync(
                WsPayFormHelper.CreateFormTransactionRequest(
                    context.GetCheckoutResponse,
                    shop,
                    context.OrderId,
                    context.AppUri
                ),
                formBaseUrl!,
                cancellationToken
            );

            if (formResponse.IsValid && formResponse.Result.HasErrors)
            {
                _logger.LogAPICommunication(Events.WSPayAPIResponseReceived, new APIRequestLogState(nameof(WSPayGateway), context.RequestId, formResponse.Result));
                return null;
            }

            return formResponse.Result?.PaymentFormUrl;
        }

        public IReadOnlyList<GetIFrameValuesResult> BuildIframeValues(GatewayCheckoutContext context)
        {
            var iframeAuthorizationUrl = context.StoreConfiguration.StoreStatus == StoreStatus.Production
                ? _formConfiguration.IframeAuthorizationUrlProd!
                : _formConfiguration.IframeAuthorizationUrlTest!;

            return context.Shops.Select(shop => WsPayFormHelper.CreateIframeRequest(
                context.GetCheckoutResponse,
                shop,
                context.OrderId,
                context.AppUri,
                context.StoreConfiguration,
                iframeAuthorizationUrl,
                ResolveIframeResponseTarget(shop)
            )).ToList();
        }
    }
}
