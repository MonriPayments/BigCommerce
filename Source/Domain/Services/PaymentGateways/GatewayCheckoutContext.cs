using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Services.RestManagementApi.Interaction;
using System.Collections.Generic;

namespace BigCommerceApi.Domain.Services.PaymentGateways
{
    public sealed class GatewayCheckoutContext
    {
        public GetCheckoutResponse.ServiceResponse GetCheckoutResponse { get; set; } = null!;
        public IReadOnlyList<BigCommerceApi.Domain.Model.Shops.Shops> Shops { get; set; } = new List<BigCommerceApi.Domain.Model.Shops.Shops>();
        public StoreConfiguration StoreConfiguration { get; set; } = null!;
        public string OrderId { get; set; } = null!;
        public string AppUri { get; set; } = null!;
        public string? RequestId { get; set; }
    }
}
