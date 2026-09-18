using BigCommerceApi.Domain.Model.Shops;
using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Services.Auth;
using BigCommerceApi.Domain.Services.Cache;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
using BigCommerceApi.Domain.Services.RestManagementApi;
using BigCommerceApi.Domain.Services.RestManagementApi.Interaction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Common.Contracts;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Domain.Services.Orders
{
    public class UpdateOrderHandler : BaseCommandHandler, IAsyncCommandHandler<UpdateOrderCommand, Response<UpdateOrderResult>>
    {
        private readonly IRestManagementApi _restManagementApi;
        private readonly IWebAppCache _memoryCache;
        private readonly ILogger _logger;
        private const string PaymentMethodDesc = "WSPay by Monri";

        public UpdateOrderHandler(IQueryExecutor queryExecutor, UnitOfWork unitOfWork, IRestManagementApi restManagementApi, IWebAppCache memoryCache, ILogger logger) : base(queryExecutor, unitOfWork)
        {
            Argument.IsNotNull(restManagementApi, nameof(restManagementApi));
            Argument.IsNotNull(memoryCache, nameof(memoryCache));

            _restManagementApi = restManagementApi;
            _memoryCache = memoryCache;
            _logger = logger;

        }

        public async Task<Response<UpdateOrderResult>> ExecuteAsync(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var response = new Response();

                var cachedValues = GetCacheStore(command.OrderId);

                var shop = await QueryExecutor.GetOneAsync<BigCommerceApi.Domain.Model.Shops.Shops>(_ => _.ShopID == cachedValues.Shopid, cancellationToken);

                var storeConfiguration = await QueryExecutor.GetOneAsync<StoreConfiguration>(_ => _.Id == shop!.StoreConfiguration!.Id, cancellationToken);

                _logger.LogAPICommunication(
                    Events.UpdateOrderHandler,
                    new APIRequestLogState(
                        nameof(AuthHandler), cachedValues.Shopid,
                        new LogDto
                        {
                            Id = command.OrderId,
                            AuthToken = storeConfiguration?.AccessToken!,
                            PaymentMethod = PaymentMethodDesc,
                            ApiPath = storeConfiguration?.ApiPath!,
                            StatusId = OrderStatus.AwaitingFulfullment
                        }
                    )
                );

                int orderStatus = storeConfiguration?.DefaultOrderStatus == null ? (int)OrderStatus.AwaitingFulfullment : (int)storeConfiguration.DefaultOrderStatus;

                UpdateOrderRequest updateOrderRequest = new UpdateOrderRequest { StatusId = orderStatus, PaymentMethod = PaymentMethodDesc };

                _logger.LogAPICommunication(
                    Events.UpdateOrderHandler,
                    new APIRequestLogState(
                        nameof(AuthHandler), cachedValues.Shopid,
                        JsonConvert.SerializeObject(updateOrderRequest)
                    )
                );

                var updateOrderResponse = await _restManagementApi.UpdateOrderResponseAsync(
                    updateOrderRequest,
                    storeConfiguration?.ApiPath!,
                    command?.OrderId!,
                    storeConfiguration?.AccessToken!,
                    new Guid().ToString(),
                    cancellationToken
                );

                _logger.LogAPICommunication(Events.UpdateOrderResponse, new APIRequestLogState(nameof(AuthHandler), cachedValues.Shopid, JsonConvert.SerializeObject(updateOrderResponse)));

                return new Response<UpdateOrderResult?>(new UpdateOrderResult { 
                    ActionSuccess = 1, 
                    SiteUrl = storeConfiguration?.MerchantSiteUrl!,
                    Currency = string.IsNullOrEmpty(cachedValues.Currency) ? "EUR" : cachedValues.Currency,
                    MerchantEmail = storeConfiguration?.MerchantEmail!,
                    MerchantName = storeConfiguration?.MerchantName!,
                    RedirectUrlRoute = storeConfiguration?.OrderConfirmationRedirectRoute
                })!;
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "UpdateOrderHandler" },
                };
                _logger.LogApplicationError(ex, additionalData);
                return ResponseHelper.CreateErrorResponse<UpdateOrderResult>(ErrorMessages.GenericError);
            }
        }

        private CacheStore GetCacheStore(string orderId)
        {
            var cachedValues = new CacheStore();
            if (_memoryCache.TryGetValue(orderId, out var cachedData))
            {
                cachedValues = cachedData as CacheStore;
                if (cachedValues == null)
                    cachedValues = new CacheStore();
            }

            return cachedValues;
        }
    }

    public class LogDto
    {
        public string? Id { get; set; }
        public string? AuthToken { get; set; }
        public string? PaymentMethod { get; set; }
        public string? ApiPath { get; set; }
        public OrderStatus? StatusId { get; set; }
    }
}
