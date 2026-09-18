using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Services.MerchantProperties
{
    public class SaveMerchantPropertiesCommand : ICommand<Response<SaveMerchantPropertiesResult>>
    {
        public string? MerchantSite { get; set; }
        public string? MerchantName { get; set; }
        public string? MerchantEmail { get; set; }
        public string? MerchantPath { get; set; }
        public string? RequestId { get; set; }
        public string? CheckoutType { get; set; }
        public string? PaymentMethodName { get; set; }
        public bool IsInProduction { get; set; }
    }
}
