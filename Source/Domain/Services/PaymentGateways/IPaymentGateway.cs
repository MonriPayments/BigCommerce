using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Services.Checkout.GetIFrameValues;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BigCommerceApi.Domain.Services.PaymentGateways
{
    public interface IPaymentGateway
    {
        PaymentGateway Gateway { get; }
        string PaymentMethodDescription { get; }

        Task<string?> CreateRedirectFormUrlAsync(GatewayCheckoutContext context, CancellationToken cancellationToken);
        IReadOnlyList<GetIFrameValuesResult> BuildIframeValues(GatewayCheckoutContext context);
    }
}
