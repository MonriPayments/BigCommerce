using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;
using BigCommerceApi.Domain.Model.Transactions;
using BigCommerceApi.Domain.Services.Logging;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Model.Shops;
using BigCommerceApi.Domain.Model.Users;
using System.Linq;
using BigCommerceApi.Domain.Services.PaymentGateways;
using BigCommerceApi.Domain.Services.RestManagementApi.Enums;
using BigCommerceApi.Domain.Services.RestManagementApi.Interaction;
using BigCommerceApi.Domain.Services.RestManagementApi;
using WebStudio.Common.Contracts;
using Newtonsoft.Json;

namespace BigCommerceApi.Domain.Services.Callback.SaveTransaction
{
    public sealed class SaveCallbackHandler : BaseCommandHandler, IAsyncCommandHandler<SaveCallbackCommand, Response<CreateOrUpdateEntityResult?>>
    {
        private readonly ILogger _logger;
        private readonly IRestManagementApi _restManagementApi;
        private readonly IPaymentGatewayResolver _paymentGatewayResolver;

        public SaveCallbackHandler(IQueryExecutor queryExecutor, UnitOfWork unitOfWork, ILogger logger, IRestManagementApi restManagementApi, IPaymentGatewayResolver paymentGatewayResolver) : base(queryExecutor, unitOfWork)
        {
            Argument.IsNotNull(restManagementApi, nameof(restManagementApi));
            Argument.IsNotNull(paymentGatewayResolver, nameof(paymentGatewayResolver));
            Argument.IsNotNull(logger, nameof(logger));

            _logger = logger;
            _restManagementApi = restManagementApi;
            _paymentGatewayResolver = paymentGatewayResolver;
        }

        public async Task<Response<CreateOrUpdateEntityResult?>> ExecuteAsync(SaveCallbackCommand command, CancellationToken cancellationToken)
        {
            var response = new Response();
                  
            var transaction = await QueryExecutor.GetOneAsync<Transaction>(_ => _.ShoppingCartID == command.ShoppingCartID && _.ShopID == command.ShopID);

            if (transaction == null)
                transaction = new Transaction();

            TransactionsHelpers.PopulateTransaction(command, ref transaction);
            
            WSPayTransactionStatus transactionStatus = TransactionsHelpers.CheckForStatusChangeAction(command, transaction);

            try
            {
                UnitOfWork.Begin();
                UnitOfWork.RegisterEntityToAddOrUpdate(transaction);

                if (!string.IsNullOrEmpty(command.CustomerEmail) && !string.IsNullOrEmpty(command.Token))
                {
                    var shops = await QueryExecutor.GetOneAsync<BigCommerceApi.Domain.Model.Shops.Shops>(_ => _.ShopID == command.ShopID);
                    var storeConfiguration = await QueryExecutor.GetOneAsync<StoreConfiguration>(_ => _.Id == shops!.StoreConfiguration!.Id);
                    var user = await QueryExecutor.GetOneAsync<User>(_ => _.Email == command.CustomerEmail && _.StoreConfiguration!.Id == storeConfiguration!.Id);

                    if (user == null)
                    {
                        user = new User();
                        UserHelper.PopulateUser(storeConfiguration!, command.CustomerEmail, ref user);
                        UnitOfWork.RegisterEntityToAddOrUpdate(user);
                    }

                    var userTokens = await QueryExecutor.GetAllAsync<UserTokens>(_ => _.User == user);
                    var userToken = userTokens.FirstOrDefault(_ => _.PaymentType == command.CreditCardName && _.MaskedPan == command.CreditCardNumber) ?? new UserTokens();

                    UserHelper.PopulateUserTokens(command, user, ref userToken);
                    UnitOfWork.RegisterEntityToAddOrUpdate(userToken);
                }

                await UnitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                var additionalData = new Dictionary<string, string>
                {
                    { "SaveCallbackHandler", ex.Message }
                };
                _logger.LogApplicationError(ex, additionalData);

                response.AddError($"Error saving to database, message: {ex.Message}, inner exception: {ex.InnerException}");
                return Response<CreateOrUpdateEntityResult?>.From(response);
            }

            try
            {
                UpdateOrderRequest updateOrderRequest = new UpdateOrderRequest();

                switch (transactionStatus)
                {
                    case WSPayTransactionStatus.Voided:
                        updateOrderRequest.StatusId = (int)OrderStatus.Cancelled;
                        break;
                    case WSPayTransactionStatus.Refunded:
                        updateOrderRequest.StatusId = (int)OrderStatus.Refunded;
                        break;
                    case WSPayTransactionStatus.Completed:
                        updateOrderRequest.StatusId = (int)OrderStatus.Completed;
                        break;
                    case WSPayTransactionStatus.PartiallyRefunded:
                        updateOrderRequest.StatusId = (int)OrderStatus.PartiallyRefunded;
                        break;
                    default:
                        return Response<CreateOrUpdateEntityResult?>.From(response, new CreateOrUpdateEntityResult(transaction.Id));
                }

                if (updateOrderRequest.StatusId == (int)OrderStatus.PartiallyRefunded || updateOrderRequest.StatusId == (int)OrderStatus.Refunded)
                {
                    updateOrderRequest.TotalIncTax = transaction.Amount.ToString();
                    updateOrderRequest.TotalExTax = transaction.Amount.ToString();
                    updateOrderRequest.RefundedAmount = (transaction.OriginalTransactionAmount - transaction.Amount).ToString();
                }

                var shop = await QueryExecutor.GetOneAsync<BigCommerceApi.Domain.Model.Shops.Shops>(_ => _.ShopID == command.ShopID, cancellationToken);

                var storeConfiguration = await QueryExecutor.GetOneAsync<StoreConfiguration>(_ => _.Id == shop!.StoreConfiguration!.Id, cancellationToken);

                updateOrderRequest.PaymentMethod = _paymentGatewayResolver.Resolve(storeConfiguration!).PaymentMethodDescription;

                _logger.LogAPICommunication(
                    Events.UpdateOrderOnCallback,
                    new APIRequestLogState(
                        nameof(SaveCallbackHandler), command.ShopID,
                        JsonConvert.SerializeObject(updateOrderRequest)
                    )
                );

                var updateOrderResponse = await _restManagementApi.UpdateOrderResponseAsync(
                        updateOrderRequest,
                        storeConfiguration?.ApiPath!,
                        command?.ShoppingCartID!,
                        storeConfiguration?.AccessToken!,
                        new Guid().ToString(),
                        cancellationToken
                );

                _logger.LogAPICommunication(
                    Events.UpdateOrderOnCallback,
                    new APIRequestLogState(
                        nameof(SaveCallbackHandler), command.ShopID,
                        JsonConvert.SerializeObject(updateOrderResponse)
                    )
                );

            }
            catch (Exception ex)
            {
                var additionalData = new Dictionary<string, string>
                {
                    { "SaveCallbackHandler", ex.Message }
                };
                _logger.LogApplicationError(ex, additionalData);

                response.AddError($"Error updating status on BigCommerce, message: {ex.Message}, inner exception: {ex.InnerException}");
                return Response<CreateOrUpdateEntityResult?>.From(response);
            }

            return Response<CreateOrUpdateEntityResult?>.From(response, new CreateOrUpdateEntityResult(transaction.Id));
        }

    }
}
