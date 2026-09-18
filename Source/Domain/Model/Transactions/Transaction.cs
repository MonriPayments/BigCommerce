using System;
using WebStudio.Entities.Core;

namespace BigCommerceApi.Domain.Model.Transactions
{
    public class Transaction : Entity
    {
        public string? WsPayOrderId { get; set; }
        public int? UniqueTransactionNumber { get; set; }
        public string? Signature { get; set; }
        public string? STAN { get; set; }
        public string? ApprovalCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ShopID { get; set; }
        public string? ShoppingCartID { get; set; }
        public double? Amount { get; set; }
        public int? CurrencyCode { get; set; }
        public bool? Success { get; set; }
        public bool? Authorized { get; set; }
        public bool? Completed { get; set; }
        public bool? Voided { get; set; }
        public bool? Refunded { get; set; }
        public string? PaymentPlan { get; set; }
        public string? Partner { get; set; }
        public string? OnSite { get; set; }
        public string? CreditCardName { get; set; }
        public string? CreditCardNumber { get; set; }
        public string? ECI { get; set; }
        public string? CustomerFirstName { get; set; }
        public string? CustomerLastName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerCity { get; set; }
        public string? CustomerCountry { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerZIP { get; set; }
        public string? CustomerEmail { get; set; }
        public DateTime? TransactionDateTime { get; set; }
        public string? Token { get; set; }
        public string? TokenNumber { get; set; }
        public string? ExpirationDate { get; set; }
        public double? OriginalTransactionAmount { get; set; }
    }
}
