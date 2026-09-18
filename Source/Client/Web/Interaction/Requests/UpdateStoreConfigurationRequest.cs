using BigCommerceApi.Client.Web.Interaction.Responses;
using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Services.StoreConfigurations.AddShopToStoreConfiguration;
using BigCommerceApi.Domain.Services.StoreConfigurations.EditStoreConfiguration;
using Newtonsoft.Json;

namespace BigCommerceApi.Client.Web.Interaction.Requests
{
    public class UpdateStoreConfigurationRequest : BaseAPIResponse<UpdateStoreConfigurationCommand>
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("accessToken")]
        public string? AccessToken { get; set; }
        [JsonProperty("clientName")]
        public string? ClientName { get; set; }
        [JsonProperty("clientID")]
        public string? ClientID { get; set; }
        [JsonProperty("clientSecret")]
        public string? ClientSecret { get; set; }
        [JsonProperty("storeApiAccountName")]
        public string? StoreApiAccountName { get; set; }
        [JsonProperty("apiPath")]
        public string? ApiPath { get; set; }
        [JsonProperty("merchantSiteUrl")]
        public string? MerchantSiteUrl { get; set; }
        [JsonProperty("subjectPath")]
        public string? SubjectPath { get; set; }
        [JsonProperty("merchantEmail")]
        public string? MerchantEmail { get; set; }
        [JsonProperty("merchantName")]
        public string? MerchantName { get; set; }
        [JsonProperty("paymentMethodName")]
        public string? PaymentMethodName { get; set; }
        [JsonProperty("storeStatus")]
        public string? StoreStatus { get; set; }
        [JsonProperty("checkoutType")]
        public string? CheckoutType { get; set; }
        [JsonProperty("paymentGateway")]
        public string? PaymentGateway { get; set; }
        [JsonProperty("paymentConfiguration")]
        public string? PaymentConfiguration { get; set; }
        [JsonProperty("orderConfirmationRedirectRoute")]
        public string? OrderConfirmationRedirectRoute { get; set; }
        [JsonProperty("defaultOrderStatus")]
        public string? DefaultOrderStatus { get; set; }
        [JsonProperty("idShop")]
        public Guid? IdShop { get; set; }

        protected override void PopulateDomainResponse(UpdateStoreConfigurationCommand domainResponse)
        {
        }
    }
}
