using System;

namespace BigCommerceApi.Domain.Projections.Transactions
{
    public class TransactionProjection
    {
        public int? UniqueTransactionNumber { get; set; }
        public string WsPayOrderId { get; set; }
        public string CreditCardName { get; set; }
        public int? CurrencyCode { get; set; }
        public double? Amount { get; set; }
        //public double? FinalAmount { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string ShopID { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerName => CustomerFirstName + " " + CustomerLastName;
        public bool? Authorized { get; set; }
        public bool? Completed { get; set; }
        public bool? Voided { get; set; }
        public bool? Refunded { get; set; }
        public string ShoppingCartID { get; set; }
    }
}
