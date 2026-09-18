using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Services.Cache;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
using BigCommerceApi.Domain.Services.PaymentGateways;
using BigCommerceApi.Domain.Services.RestManagementApi;
using BigCommerceApi.Domain.Services.RestManagementApi.Interaction;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Common.Contracts;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Domain.Services.Checkout.InitiateCheckout
{
    public class InitiateCheckoutHandler : BaseCommandHandler, IAsyncCommandHandler<InitiateCheckoutCommand, Response<InitiateCheckoutResult>>
    {
        private readonly IRestManagementApi _restManagementApi;
        private readonly IPaymentGatewayResolver _paymentGatewayResolver;
        private readonly IWebAppCache _memoryCache;
        private readonly BigCommerceConfiguration _bigCommerceConfiguration;
        private readonly ILogger _logger;
        const int maxRetries = 15;
        const int delayMilliseconds = 2000;

        public InitiateCheckoutHandler(
            IQueryExecutor queryExecutor,
            UnitOfWork unitOfWork,
            IRestManagementApi restManagementApi,
            IPaymentGatewayResolver paymentGatewayResolver,
            IWebAppCache memoryCache,
            BigCommerceConfiguration bigCommerceConfiguration,
            ILogger logger) : base(queryExecutor, unitOfWork)
        {
            Argument.IsNotNull(restManagementApi, nameof(restManagementApi));
            Argument.IsNotNull(paymentGatewayResolver, nameof(paymentGatewayResolver));
            Argument.IsNotNull(memoryCache, nameof(memoryCache));
            Argument.IsNotNull(bigCommerceConfiguration, nameof(bigCommerceConfiguration));
            Argument.IsNotNull(logger, nameof(logger));

            _restManagementApi = restManagementApi;
            _paymentGatewayResolver = paymentGatewayResolver;
            _memoryCache = memoryCache;
            _bigCommerceConfiguration = bigCommerceConfiguration;
            _logger = logger;
        }

        public async Task<Response<InitiateCheckoutResult>> ExecuteAsync(InitiateCheckoutCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var storeConfiguration = await QueryExecutor.GetOneAsync<StoreConfiguration>(_ => _.MerchantSiteUrl == command.SiteUrl, cancellationToken);

                var getCheckoutResponse = await _restManagementApi.GetCheckoutResponseAsync(storeConfiguration!.ApiPath!, command!.CheckoutID!, storeConfiguration!.AccessToken!, command!.RequestId, cancellationToken);

                if (getCheckoutResponse == null)
                    return ResponseHelper.CreateErrorResponse<InitiateCheckoutResult>(ErrorMessages.StoreConfigurationNotFound);

                CheckoutCreateOrderResponse.ServiceResponse? checkoutResponse = null;

                for (int attempt = 1; attempt <= maxRetries; attempt++)
                {
                    checkoutResponse = await _restManagementApi.GetCheckoutCreateOrderResponseAsync(
                        storeConfiguration!.ApiPath!,
                        command!.CheckoutID!,
                        storeConfiguration!.AccessToken!,
                        command!.RequestId,
                        cancellationToken
                    );

                    if (checkoutResponse?.Data != null)
                    {
                        break;
                    }

                    if (attempt < maxRetries)
                    {
                        _logger.LogAPICommunication(Events.BigCCreateOrderFromCartFailed, new APIRequestLogState(nameof(InitiateCheckoutHandler), command.RequestId, $"Attempt {attempt} failed. Response or Data was null. Retrying in {delayMilliseconds}ms..."));

                        await Task.Delay(delayMilliseconds, cancellationToken);
                    }
                }

                if (checkoutResponse?.Data == null)
                {
                    var errorMessage = $"{ErrorMessages.CheckoutProcessFailed} , {checkoutResponse?.Error}";
                    return ResponseHelper.CreateErrorResponse<InitiateCheckoutResult>(errorMessage);
                }

                _logger.LogAPICommunication(Events.WSPayAPIResponseReceived, new APIRequestLogState(nameof(InitiateCheckoutHandler), command.RequestId, ToLog(getCheckoutResponse)));

                var shop = await QueryExecutor.GetOneAsync<BigCommerceApi.Domain.Model.Shops.Shops>(_ => _.StoreConfiguration == storeConfiguration && _.IsTokenShopId == false, cancellationToken);
                if (shop == null)
                    return ResponseHelper.CreateErrorResponse<InitiateCheckoutResult>(ErrorMessages.ShopIdNotFound);

                var orderId = checkoutResponse.Data.Id!.ToString()!;
                var gateway = _paymentGatewayResolver.Resolve(storeConfiguration);

                var formUrl = await gateway.CreateRedirectFormUrlAsync(
                    new GatewayCheckoutContext
                    {
                        GetCheckoutResponse = getCheckoutResponse,
                        Shops = new List<BigCommerceApi.Domain.Model.Shops.Shops> { shop },
                        StoreConfiguration = storeConfiguration,
                        OrderId = orderId,
                        AppUri = _bigCommerceConfiguration.AppUri!,
                        RequestId = command.RequestId
                    },
                    cancellationToken
                );

                if (string.IsNullOrEmpty(formUrl))
                    return ResponseHelper.CreateErrorResponse<InitiateCheckoutResult>(ErrorMessages.WSPayFormTransactionNotCreated);

                SaveToCache(shop.ShopID!, orderId, getCheckoutResponse.Data!.Cart!.Currency!.Code!, command.SiteUrl);

                return new Response<InitiateCheckoutResult>(new InitiateCheckoutResult { FormUrl = formUrl });
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "InitiateCheckoutHandler" },
                };
                _logger.LogApplicationError(ex, additionalData);
                return ResponseHelper.CreateErrorResponse<InitiateCheckoutResult>(ErrorMessages.GenericError);
            }
        }

        private void SaveToCache(string shopId, string orderId, string currencyCode, string siteUrl)
        {
            var cacheStore = new CacheStore();

            cacheStore.Shopid = shopId;
            cacheStore.Currency = currencyCode;
            cacheStore.SiteUrl = siteUrl;

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_memoryCache.Timeout.GetValueOrDefault(30))
            };
            var cacheKey = orderId;
            _memoryCache.Set(cacheKey, cacheStore, cacheEntryOptions);
        }

        private object ToLog(GetCheckoutResponse.ServiceResponse getCheckoutResponse)
        {
            return new
            {
               Amount = getCheckoutResponse.Data.GrandTotal,
               id = getCheckoutResponse.Data.Id
            };
        }
    }
}
