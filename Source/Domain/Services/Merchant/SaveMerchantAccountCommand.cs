using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Services.Merchant
{
    public class SaveMerchantAccountCommand : ICommand<Response<SaveMerchantAccountResult>>
    {
        public string? ShopId { get; set; }
        public string? SecretKey { get; set; }
        public string? AccessToken { get; set; }
        public string? ClientName { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? Name { get; set; }
        public string? ApiPath { get; set; }
        public string? MerchantSiteUrl { get; set; }
    }
}
