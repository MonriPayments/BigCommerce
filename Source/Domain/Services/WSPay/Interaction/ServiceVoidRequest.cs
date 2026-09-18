namespace BigCommerceApi.Domain.Services.WSPay.Interaction
{
    public class ServiceVoidRequest : IServiceRequest
    {
        public string? WsPayOrderId { get; set; }
        public string? Signature { get; set; }
        public string? STAN { get; set; }
        public string? ApprovalCode { get; set; }
        public string? Amount { get; set; }
        public string? ShopID { get; set; }
        public string? Version { get; set; } = "2.0";
        public MarketplaceRequest? Marketplace { get; set; }
    }
}
