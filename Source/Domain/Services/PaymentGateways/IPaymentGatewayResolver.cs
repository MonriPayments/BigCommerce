using BigCommerceApi.Domain.Model.Stores;

namespace BigCommerceApi.Domain.Services.PaymentGateways
{
    public interface IPaymentGatewayResolver
    {
        IPaymentGateway Resolve(StoreConfiguration storeConfiguration);
    }
}
