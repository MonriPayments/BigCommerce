using BigCommerceApi.Domain.Services.Merchant;

namespace BigCommerceApi.Client.Web.Interaction.Requests
{
    public class SaveMerchantAccount : BaseAPIRequest<SaveMerchantAccountCommand>
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
        protected override void PopulateDomainRequest(SaveMerchantAccountCommand domainRequest)
        {
            domainRequest.ShopId = ShopId;
            domainRequest.SecretKey = SecretKey;
            domainRequest.AccessToken = AccessToken;
            domainRequest.ClientName = ClientName;
            domainRequest.ClientId = ClientId;
            domainRequest.ClientSecret = ClientSecret;
            domainRequest.Name = Name;
            domainRequest.ApiPath = ApiPath;
            domainRequest.MerchantSiteUrl = MerchantSiteUrl;
        }
    }
}
