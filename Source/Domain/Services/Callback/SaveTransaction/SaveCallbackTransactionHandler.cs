using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;
using BigCommerceApi.Domain.Model.Transactions;
using BigCommerceApi.Domain.Services.Logging;

namespace BigCommerceApi.Domain.Services.Callback.SaveTransaction
{
    public sealed class SaveCallbackTransactionHandler : BaseCommandHandler, IAsyncCommandHandler<SaveCallbackTransactionCommand, Response<CreateOrUpdateEntityResult?>>
    {
        private readonly ILogger _logger;
        public SaveCallbackTransactionHandler(IQueryExecutor queryExecutor, UnitOfWork unitOfWork, ILogger logger) : base(queryExecutor, unitOfWork)
        {
            _logger = logger;
        }

        public async Task<Response<CreateOrUpdateEntityResult?>> ExecuteAsync(SaveCallbackTransactionCommand command, CancellationToken cancellationToken)
        {
            var response = new Response();

            var transaction = await QueryExecutor.GetOneAsync<Transaction>(_ => _.ShoppingCartID == command.ShoppingCartID && _.ShopID == command.ShopID);

            if (transaction == null)
                transaction = new Transaction();

            try
            {
                transaction.WsPayOrderId = command.WsPayOrderId;
                transaction.UniqueTransactionNumber = command.UniqueTransactionNumber;
                transaction.Signature = command.Signature;
                transaction.STAN = command.STAN;
                transaction.ApprovalCode = command.ApprovalCode;
                transaction.ErrorMessage = command.ErrorMessage;
                transaction.ShopID = command.ShopID;
                transaction.ShoppingCartID = command.ShoppingCartID;
                transaction.Amount = command.Amount;
                transaction.CurrencyCode = command.CurrencyCode;
                transaction.Success = ToPgBool(command.Success);
                transaction.Authorized = ToPgBool(command.Authorized);
                transaction.Completed = ToPgBool(command.Completed);
                transaction.Voided = ToPgBool(command.Voided);
                transaction.Refunded = ToPgBool(command.Refunded);
                transaction.PaymentPlan = command.PaymentPlan;
                transaction.Partner = command.Partner;
                transaction.OnSite = command.OnSite;
                transaction.CreditCardName = command.CreditCardName;
                transaction.CreditCardNumber = command.CreditCardNumber;
                transaction.ECI = command.ECI;
                transaction.CustomerFirstName = command.CustomerFirstName;
                transaction.CustomerLastName = command.CustomerLastName;
                transaction.CustomerAddress = command.CustomerAddress;
                transaction.CustomerCity = command.CustomerCity;
                transaction.CustomerCountry = command.CustomerCountry;
                transaction.CustomerPhone = command.CustomerPhone;
                transaction.CustomerZIP = command.CustomerZIP;
                transaction.CustomerEmail = command.CustomerEmail;
                transaction.ErrorMessage = "";
                transaction.TransactionDateTime = DateTime.ParseExact(command.TransactionDateTime, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None);
                transaction.Token = command.Token;
                transaction.TokenNumber = command.TokenNumber;
                transaction.ExpirationDate = command.ExpirationDate;

                UnitOfWork.Begin();
                UnitOfWork.RegisterEntityToAddOrUpdate(transaction);
                await UnitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                var additionalData = new Dictionary<string, string>
                {
                    {"Message", ex.Message }
                };
                _logger.LogApplicationError(ex, additionalData);

                response.AddError($"Error saving to database, message: {ex.Message}, inner exception: {ex.InnerException}");
                return Response<CreateOrUpdateEntityResult?>.From(response);
            }


            return Response<CreateOrUpdateEntityResult?>.From(response, new CreateOrUpdateEntityResult(transaction.Id));
        }

        public static bool ToPgBool(string? pgBool)
        {
            if (pgBool == "1")
                return true;
            
            return false;
            
        }
    }
}
