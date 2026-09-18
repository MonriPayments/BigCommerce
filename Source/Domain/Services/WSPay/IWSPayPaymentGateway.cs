using BigCommerceApi.Domain.Services.WSPay.Interaction;
using System.Threading;
using System.Threading.Tasks;

namespace BigCommerceApi.Domain.Services.WSPay
{
    public interface IWSPayPaymentGateway
    {
        public Task<ServiceCompletionResponse> ServiceCompletionAsync(ServiceCompletionRequest request, string? requestId, CancellationToken cancellationToken);
        public Task<ServiceRefundResponse> ServiceRefundAsync(ServiceRefundRequest request, string? requestId, CancellationToken cancellationToken);
        public Task<ServiceVoidResponse> ServiceVoidAsync(ServiceVoidRequest request, string? requestId, CancellationToken cancellationToken);
    }
}
