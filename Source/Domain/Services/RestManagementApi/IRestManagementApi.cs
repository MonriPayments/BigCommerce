using BigCommerceApi.Domain.Services.RestManagementApi.Interaction;
using System.Threading;
using System.Threading.Tasks;

namespace BigCommerceApi.Domain.Services.RestManagementApi
{
    public interface IRestManagementApi
    {
        public Task<GetCheckoutResponse.ServiceResponse> GetCheckoutResponseAsync(string merchantApiPath, string checkoutId, string authToken, string? requestId, CancellationToken cancellationToken);
        public Task<CheckoutCreateOrderResponse.ServiceResponse> GetCheckoutCreateOrderResponseAsync(string merchantApiPath, string checkoutId, string authToken, string? requestId, CancellationToken cancellationToken);
        public Task<UpdateOrderResponse.ServiceResponse> UpdateOrderResponseAsync(UpdateOrderRequest request, string merchantApiPath, string orderId, string authToken, string? requestId, CancellationToken cancellationToken);
        public Task<CreateScriptResponse.ServiceResponse> CreateScriptResponseAsync(CreateScriptRequest request, string merchantApiPath, string authToken, string? requestId, CancellationToken cancellationToken);
        public Task<CreateWebhookResponse.ServiceResponse> CreateWebhookResponseAsync(CreateWebhookRequest request, string merchantApiPath, string authToken, string? requestId, CancellationToken cancellationToken);
        public Task<GetAccessTokenResponse.ServiceResponse> GetAccessTokenResponseAsync(GetAccessTokenRequest request, string authPath, string? requestId, CancellationToken cancellationToken);
    }
}
