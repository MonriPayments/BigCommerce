using System.Collections.Generic;

namespace BigCommerceApi.Domain.Services.Transactions
{
    public class TransactionResult
    {
        public int? UniqueTransactionNumber { get; set; }
        public string WsPayOrderId { get; set; }
        public string CreditCardName { get; set; }
        public int? CurrencyCode { get; set; }
        public double? Amount { get; set; }
    }

    public class GetTransactionsResult
    {
        public List<TransactionResult>? TransactionsResult { get; set; }
    }
}
