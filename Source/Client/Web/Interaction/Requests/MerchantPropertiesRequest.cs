using BigCommerceApi.Domain.Services.MerchantProperties;

namespace BigCommerceApi.Client.Web.Interaction.Requests
{
    public class MerchantPropertiesRequest : BaseAPIRequest<SaveMerchantPropertiesCommand>
    {
        public string? MerchantSite { get; set; }
        public string? MerchantName { get; set; }
        public string? MerchantEmail { get; set; }
        public string? MerchantPath { get; set; }
        public string? CheckoutType { get; set; }
        public string? PaymentMethodName { get; set; }
        public bool IsProduction { get; set; }

        protected override void PopulateDomainRequest(SaveMerchantPropertiesCommand domainRequest)
        {
            domainRequest.MerchantSite = MerchantSite;
            domainRequest.MerchantName = MerchantName;
            domainRequest.MerchantEmail = MerchantEmail;
            domainRequest.MerchantPath = MerchantPath;
            domainRequest.IsInProduction = IsProduction;
            domainRequest.CheckoutType = CheckoutType;
            domainRequest.PaymentMethodName = PaymentMethodName;
        }
    }
}
