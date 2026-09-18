using Newtonsoft.Json;

namespace BigCommerceApi.Client.Web.Interaction.Responses
{
    public class GetIframeValuesResponse
    {
        [JsonProperty("url")]
        public string? Url { get; set; }

        [JsonProperty("shopID")]
        public string? ShopID { get; set; }

        [JsonProperty("shoppingCartID")]
        public string? ShoppingCartID { get; set; }

        [JsonProperty("version")]
        public string? Version { get; set; }

        [JsonProperty("totalAmount")]
        public string? TotalAmount { get; set; }

        [JsonProperty("signature")]
        public string? Signature { get; set; }

        [JsonProperty("returnURL")]
        public string? ReturnURL { get; set; }

        [JsonProperty("cancelURL")]
        public string? CancelURL { get; set; }

        [JsonProperty("returnErrorURL")]
        public string? ReturnErrorURL { get; set; }

        [JsonProperty("iFrame")]
        public string? IFrame { get; set; }

        [JsonProperty("iFrameResponseTarget")]
        public string? IFrameResponseTarget { get; set; }

        [JsonProperty("isTokenRequest")]
        public string? IsTokenRequest { get; set; }

        [JsonProperty("isTokenShopID")]
        public bool? IsTokenShopID { get; set; }

        [JsonProperty("lang")]
        public string? Lang { get; set; }

        [JsonProperty("customerFirstName")]
        public string? CustomerFirstName { get; set; }

        [JsonProperty("customerLastName")]
        public string? CustomerLastName { get; set; }

        [JsonProperty("customerAddress")]
        public string? CustomerAddress { get; set; }

        [JsonProperty("customerCity")]
        public string? CustomerCity { get; set; }

        [JsonProperty("customerZIP")]
        public string? CustomerZIP { get; set; }

        [JsonProperty("customerCountry")]
        public string? CustomerCountry { get; set; }

        [JsonProperty("customerEmail")]
        public string? CustomerEmail { get; set; }

        [JsonProperty("customerPhone")]
        public string? CustomerPhone { get; set; }

        [JsonProperty("currencyCode")]
        public string? CurrencyCode { get; set; }

        [JsonProperty("paymentPlan")]
        public string? PaymentPlan { get; set; }
    }
}
