using BigCommerceApi.Domain.Model.Transactions;
using BigCommerceApi.Domain.Services.Callback.SaveTransaction;
using BigCommerceApi.Domain.Services.RestManagementApi.Enums;
using System;
using System.Collections.Generic;

namespace BigCommerceApi.Domain.Services.Helpers
{
    public class TransactionsHelpers
    {
        public static List<Transactions.TransactionResult> GetTransactionList(List<Transaction> model)
        {
            var transactionList = new List<Transactions.TransactionResult>();

            foreach (var transaction in model)
            {
                Transactions.TransactionResult transactionResult = new Transactions.TransactionResult
                {
                    UniqueTransactionNumber = transaction?.UniqueTransactionNumber,
                    WsPayOrderId = transaction.WsPayOrderId,
                    CreditCardName = transaction.CreditCardName,
                    CurrencyCode = transaction?.CurrencyCode,
                    Amount = transaction?.Amount,
                };

                transactionList.Add(transactionResult);
            }

            return transactionList;
        }

        public static void PopulateTransaction(SaveCallbackCommand command, ref Transaction transaction)
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
            transaction.TransactionDateTime = DateTime.ParseExact(command.TransactionDateTime!, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None);
            transaction.Token = command.Token;
            transaction.TokenNumber = command.TokenNumber;
            transaction.ExpirationDate = command.ExpirationDate;

            if (transaction.OriginalTransactionAmount == null)
                transaction.OriginalTransactionAmount = command.Amount;
        }

        public static bool ToPgBool(string? pgBool)
        {
            if (pgBool == "1")
                return true;

            return false;

        }

        public static WSPayTransactionStatus CheckForStatusChangeAction(SaveCallbackCommand command, Transaction transaction)
        {
            if(transaction == null)
                return WSPayTransactionStatus.NoChange;

            string actionConfirmed = "1";

            if (command.Voided == actionConfirmed)
                if (transaction.Voided != ToPgBool(actionConfirmed))
                    return WSPayTransactionStatus.Voided;

            if (command.Refunded == actionConfirmed)
            {
                if (transaction.Amount == 0)
                    return WSPayTransactionStatus.Refunded;
                else
                    return WSPayTransactionStatus.PartiallyRefunded;
            }

            if (command.Completed == actionConfirmed)
                if (transaction.Completed != ToPgBool(actionConfirmed))
                    return WSPayTransactionStatus.Completed;

            return WSPayTransactionStatus.NoChange;
        }
    }
}
