using BigCommerceApi.Client.Web.Interaction.Responses;
using BigCommerceApi.Domain.Services.StoreConfigurations.AddShopToStoreConfiguration;

namespace BigCommerceApi.Client.Web.Interaction.Requests
{
    public class AddShopToStoreConfigurationRequest : BaseAPIResponse<AddShopToStoreConfigurationCommand>
    {
        public string? ShopID { get; set; }
        public string? SecretKey { get; set; }
        public string? Language { get; set; }
        public bool? IsTokenShopId { get; set; }
        public string? StoreConfigurationId { get; set; }

        protected override void PopulateDomainResponse(AddShopToStoreConfigurationCommand domainResponse)
        {
        }
    }
}
