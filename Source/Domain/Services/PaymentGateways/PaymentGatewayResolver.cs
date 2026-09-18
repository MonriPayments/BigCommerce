using BigCommerceApi.Domain.Model.Stores;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BigCommerceApi.Domain.Services.PaymentGateways
{
    public sealed class PaymentGatewayResolver : IPaymentGatewayResolver
    {
        private readonly IReadOnlyDictionary<PaymentGateway, IPaymentGateway> _gateways;

        public PaymentGatewayResolver(IEnumerable<IPaymentGateway> gateways)
        {
            _gateways = gateways.ToDictionary(g => g.Gateway);
        }

        public IPaymentGateway Resolve(StoreConfiguration storeConfiguration)
        {
            var gateway = storeConfiguration.PaymentGateway ?? PaymentGateway.WSPay;
            if (!_gateways.TryGetValue(gateway, out var implementation))
                throw new InvalidOperationException($"No payment gateway implementation registered for {gateway}.");
            return implementation;
        }
    }
}
