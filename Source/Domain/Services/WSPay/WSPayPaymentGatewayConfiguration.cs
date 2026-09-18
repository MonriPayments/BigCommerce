using System;

namespace BigCommerceApi.Domain.Services.WSPay
{
    public sealed class WSPayPaymentGatewayConfiguration
    {
        public string? BaseUrlTest { get; set; }
        public string? BaseUrlProd { get; set; }
        public string? BasicAuthUsername { get; set; }
        public string? BasicAuthPassword { get; set; }
        public int DefaultTimeoutInSeconds { get; set; }
        public TimeSpan DefaultTimeout => TimeSpan.FromSeconds(DefaultTimeoutInSeconds);
    }
}
