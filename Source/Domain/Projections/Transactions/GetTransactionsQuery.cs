using System;
using System.Collections.Generic;
using WebStudio.Entities.Interaction;


namespace BigCommerceApi.Domain.Projections.Transactions
{
    public sealed class GetTransactionsQuery : IQuery<IList<TransactionProjection>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedUntil { get; set; }
        public string CustomerName { get; set; }
        public string ShoppingCartID { get; set; }
        public string CreditCardName { get; set; }
        public string ShopID { get; set; }

        public string CustomerFirstName => string.IsNullOrEmpty(CustomerName) ? null : CustomerName.Split(" ")[0];
        public string CustomerLastName => string.IsNullOrEmpty(CustomerName) ? null : CustomerName.Split(" ")[1];
    }
}
