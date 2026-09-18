namespace BigCommerceApi.Domain.Services.WSPay.Interaction
{
    public class RelatedTransactionRequest
    {
        public string? ShopID { get; set; }
        public string? OrderID { get; set; }
        public string? Amount { get; set; }
    }
}
