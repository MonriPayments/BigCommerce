namespace BigCommerceApi.Domain.Services.WSPay.Interaction
{
    public interface IServiceResponse
    {
        Result Result { get; set; }
    }

    public class Result
    {
        public string? ActionSuccess { get; set; }
        public string? WsPayOrderId { get; set; }
        public string? Signature { get; set; }
        public string? STAN { get; set; }
        public string? ApprovalCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ShopID { get; set; }
    }

}
