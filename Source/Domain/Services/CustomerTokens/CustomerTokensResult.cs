using System.Collections.Generic;

namespace BigCommerceApi.Domain.Services.CustomerTokens
{
    public class CustomerTokensResult
    {
        public List<CustomerTokens>? CustomerTokens { get; set; }
    }

    public class CustomerTokens
    {
        public string? Token { get; set; }
        public string? TokenNumber { get; set; }
        public string? MaskedPan { get; set; }
        public string? PaymentType { get; set; }
        public string? ImageSource { get; set; }
    }
}
