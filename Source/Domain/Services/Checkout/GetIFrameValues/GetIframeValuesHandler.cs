using BigCommerceApi.Domain.Model.Shops;
using BigCommerceApi.Domain.Model.Stores;
using ILogger = WebStudio.Logging.Abstractions.ILogger;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.PaymentGateways;
using BigCommerceApi.Domain.Services.RestManagementApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Common.Contracts;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using BigCommerceApi.Domain.Services.Logging;
using Microsoft.Extensions.Caching.Memory;
using BigCommerceApi.Domain.Services.Cache;
using BigCommerceApi.Domain.Services.Auth;
using BigCommerceApi.Domain.Services.DtosForLogging;

namespace BigCommerceApi.Domain.Services.Checkout.GetIFrameValues
{
    public class GetIframeValuesHandler : BaseCommandHandler, IAsyncCommandHandler<GetIFrameValuesCommand, Response<List<GetIFrameValuesResult>>>
    {
        private readonly IRestManagementApi _restManagementApi;
        private readonly IPaymentGatewayResolver _paymentGatewayResolver;
        private readonly BigCommerceConfiguration _bigCommerceConfiguration;
        private readonly ILogger _logger;
        private readonly IWebAppCache _memoryCache;

        public GetIframeValuesHandler(
            IQueryExecutor queryExecutor,
            UnitOfWork unitOfWork,
            IRestManagementApi restManagementApi,
            IPaymentGatewayResolver paymentGatewayResolver,
            BigCommerceConfiguration bigCommerceConfiguration,
            ILogger logger,
            IWebAppCache memoryCache) : base(queryExecutor, unitOfWork)
        {
            Argument.IsNotNull(restManagementApi, nameof(restManagementApi));
            Argument.IsNotNull(paymentGatewayResolver, nameof(paymentGatewayResolver));
            Argument.IsNotNull(bigCommerceConfiguration, nameof(bigCommerceConfiguration));
            Argument.IsNotNull(logger, nameof(logger));
            Argument.IsNotNull(memoryCache, nameof(memoryCache));

            _restManagementApi = restManagementApi;
            _paymentGatewayResolver = paymentGatewayResolver;
            _bigCommerceConfiguration = bigCommerceConfiguration;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<Response<List<GetIFrameValuesResult>>> ExecuteAsync(GetIFrameValuesCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var iFrameValuesList = new List<GetIFrameValuesResult>();

                var storeConfiguration = await QueryExecutor.GetOneAsync<StoreConfiguration>(_ => _.MerchantSiteUrl == command.SiteUrl, cancellationToken);

                var getCheckoutResponse = await _restManagementApi.GetCheckoutResponseAsync(
                    storeConfiguration!.ApiPath!, 
                    command!.CheckoutID!, 
                    storeConfiguration!.AccessToken!, 
                    command!.RequestId, 
                    cancellationToken
                );

                _logger.LogAPICommunication(Events.BigCGetCheckoutResponse, new APIRequestLogState(nameof(AuthHandler), command.RequestId, PrepareForLog.PrepareGetCheckoutResponseForLogging(getCheckoutResponse)));

                if (getCheckoutResponse == null)
                    return ResponseHelper.CreateErrorResponse<List<GetIFrameValuesResult>>(ErrorMessages.StoreConfigurationNotFound);

                var getCheckoutCreateOrderResponse = await _restManagementApi.GetCheckoutCreateOrderResponseAsync(
                    storeConfiguration!.ApiPath!, 
                    command!.CheckoutID!, 
                    storeConfiguration!.AccessToken!, 
                    command!.RequestId, 
                    cancellationToken
                );

                if (getCheckoutCreateOrderResponse == null)
                {
                    _logger.LogAPICommunication(Events.BigCGetCheckoutOrderError, new APIRequestLogState(nameof(AuthHandler), command.RequestId, null));
                    return ResponseHelper.CreateErrorResponse<List<GetIFrameValuesResult>>(ErrorMessages.CheckoutProcessFailed);
                }

                if (!string.IsNullOrEmpty(getCheckoutCreateOrderResponse.Error))
                {
                    _logger.LogAPICommunication(Events.BigCGetCheckoutOrderError, new APIRequestLogState(nameof(AuthHandler), command.RequestId, PrepareForLog.PrepareCheckoutCreateOrderResponseForLogging(getCheckoutCreateOrderResponse)));
                    return ResponseHelper.CreateErrorResponse<List<GetIFrameValuesResult>>(ErrorMessages.CheckoutProcessFailed);
                }

                var shops = (await QueryExecutor.GetAllAsync<BigCommerceApi.Domain.Model.Shops.Shops>(_ => _.StoreConfiguration!.Id == storeConfiguration.Id, cancellationToken))?.ToList();
                if (shops == null || shops.Count == 0)
                    return ResponseHelper.CreateErrorResponse<List<GetIFrameValuesResult>>(ErrorMessages.ShopIdNotFound);

                var orderId = getCheckoutCreateOrderResponse?.Data?.Id?.ToString()!;
                var gateway = _paymentGatewayResolver.Resolve(storeConfiguration);

                var gatewayIframeValues = gateway.BuildIframeValues(new GatewayCheckoutContext
                {
                    GetCheckoutResponse = getCheckoutResponse,
                    Shops = shops,
                    StoreConfiguration = storeConfiguration,
                    OrderId = orderId,
                    AppUri = _bigCommerceConfiguration.AppUri!,
                    RequestId = command.RequestId
                });
                iFrameValuesList.AddRange(gatewayIframeValues);

                foreach (var shop in shops.Where(s => s.IsTokenShopId == false))
                {
                    SaveToCache(shop.ShopID!, orderId, getCheckoutResponse.Data!.Cart!.Currency!.Code!, command.SiteUrl);
                }

                return new Response<List<GetIFrameValuesResult>>(iFrameValuesList);
            }
            catch(Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "GetIframeValuesHandler" },
                };
                _logger.LogApplicationError(ex, additionalData);
                return ResponseHelper.CreateErrorResponse<List<GetIFrameValuesResult>>(ErrorMessages.GenericError);
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
    }
}
