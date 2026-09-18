using System;

namespace BigCommerceApi.Domain.Services.WSPay.Interaction
{
    public sealed class ServiceVoidResponse : WSPayAPIResponse<ServiceVoidResponse.ServiceResponse>, IServiceResponse
    {
        public ServiceVoidResponse(ServiceResponse? serviceResponse = null, Exception? raisedException = null) : base(serviceResponse, raisedException)
        {

        }

        Result IServiceResponse.Result { get; set; }

        public sealed class ServiceResponse : Result
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
}
