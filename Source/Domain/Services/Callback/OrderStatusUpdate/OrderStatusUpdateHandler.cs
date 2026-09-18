using BigCommerceApi.Domain.Model.Shops;
using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Model.Transactions;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
using BigCommerceApi.Domain.Services.WSPay;
using BigCommerceApi.Domain.Services.WSPay.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Common.Contracts;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Domain.Services.Callback.OrderStatusUpdate
{
    public class OrderStatusUpdateHandler : BaseCommandHandler, IAsyncCommandHandler<OrderStatusUpdateCommand, Response<OrderStatusUpdateResult>>
    {
        private readonly IWSPayPaymentGateway _wspayPaymentGateway;
        private readonly ILogger _logger;

        public OrderStatusUpdateHandler(IQueryExecutor queryExecutor, UnitOfWork unitOfWork, IWSPayPaymentGateway wspayPaymentGateway, ILogger logger) : base(queryExecutor, unitOfWork)
        {
            Argument.IsNotNull(wspayPaymentGateway, nameof(wspayPaymentGateway));
            Argument.IsNotNull(logger, nameof(logger));

            _wspayPaymentGateway = wspayPaymentGateway;
            _logger = logger;
        }

        public async Task<Response<OrderStatusUpdateResult?>> ExecuteAsync(OrderStatusUpdateCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var response = new Response();
                var storeConfiguration = await QueryExecutor.GetOneAsync<StoreConfiguration>(_ => _.ClientID == command.StoreId);

                if (storeConfiguration == null)
                    return ResponseHelper.CreateErrorResponse<OrderStatusUpdateResult?>(ErrorMessages.StoreConfigurationNotFound);

                var shops = await QueryExecutor.GetAllAsync<BigCommerceApi.Domain.Model.Shops.Shops>(_ => _.StoreConfiguration == storeConfiguration, cancellationToken);

                if (shops == null)
                    return ResponseHelper.CreateErrorResponse<OrderStatusUpdateResult?>(ErrorMessages.ShopIdNotFound);                

                var shop = shops.Select(s => new BigCommerceApi.Domain.Model.Shops.Shops()).Where(t => t.IsTokenShopId == false).FirstOrDefault();

                var transaction = await QueryExecutor.GetOneAsync<Transaction>(_ => _.ShoppingCartID == command.Id.ToString() && _.ShopID == shop!.ShopID);

                if (transaction == null)
                    return ResponseHelper.CreateErrorResponse<OrderStatusUpdateResult?>(ErrorMessages.TransactionNotFound);

                var orderStatusResult = await CallService(transaction, command, shop.SecretKey, cancellationToken);

                return new Response<OrderStatusUpdateResult?>(orderStatusResult);
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "OrderStatusUpdateHandler" },
                };
                _logger.LogApplicationError(ex, additionalData);
                return ResponseHelper.CreateErrorResponse<OrderStatusUpdateResult?>(ErrorMessages.GenericError);
            }
        }

        public async Task<OrderStatusUpdateResult> CallService(
            Transaction transaction,
            OrderStatusUpdateCommand command,
            string secretKey,
            CancellationToken cancellationToken)
        {
            switch (command.NewStatusId)
            {
                case OrderStatus.Refunded:
                    var serviceRefundResponse = await _wspayPaymentGateway.ServiceRefundAsync(
                        //todo check this for a call, maybe refactor
                        WSPayTransactionManagementHelpers.GenerateServiceRequest<ServiceRefundRequest>(transaction, secretKey),
                        command.RequestId,
                        cancellationToken
                        );
                    return WSPayTransactionManagementHelpers.GenerateServiceResponse<ServiceRefundResponse>(serviceRefundResponse);
                case OrderStatus.Completed:
                    var serviceCompletionResponse = await _wspayPaymentGateway.ServiceCompletionAsync(
                        WSPayTransactionManagementHelpers.GenerateServiceRequest<ServiceCompletionRequest>(transaction, secretKey),
                        command.RequestId,
                        cancellationToken
                        );
                    return WSPayTransactionManagementHelpers.GenerateServiceResponse<ServiceCompletionResponse>(serviceCompletionResponse);
                case OrderStatus.Cancelled:
                    var serviceVoidResponse = await _wspayPaymentGateway.ServiceVoidAsync(
                        WSPayTransactionManagementHelpers.GenerateServiceRequest<ServiceVoidRequest>(transaction, secretKey),
                        command.RequestId,
                        cancellationToken
                        );
                    return WSPayTransactionManagementHelpers.GenerateServiceResponse<ServiceVoidResponse>(serviceVoidResponse);
                default:
                    return null;
            }
        }
    }
}
