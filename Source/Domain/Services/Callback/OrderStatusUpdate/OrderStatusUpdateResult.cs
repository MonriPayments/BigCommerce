namespace BigCommerceApi.Domain.Services.Callback.OrderStatusUpdate
{
    public class OrderStatusUpdateResult
    {
        public string? WsPayOrderId { get; set; }
        public string? Signature { get; set; }
        public string? STAN { get; set; }
        public string? ApprovalCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ShopID { get; set; }
        public string? ActionSuccess { get; set; }
    }
}
