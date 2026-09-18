namespace BigCommerceApi.Domain.Services.Checkout.GetIFrameValues
{
    public class GetIFrameValuesResult
    {
        public string? Url { get; set; }
        public string? ShopID { get; set; }
        public string? ShoppingCartID { get; set; }
        public string? Version { get; set; }
        public string? TotalAmount { get; set; }
        public string? Signature { get; set; }
        public string? ReturnURL { get; set; }
        public string? CancelURL { get; set; }
        public string? ReturnErrorURL { get; set; }
        public string? IFrame { get; set; }
        public string? IFrameResponseTarget { get; set; }
        public string? IsTokenRequest { get; set; }
        public bool? IsTokenShopID { get; set; }
        public string? Lang { get; set; }
        public string? CustomerFirstName { get; set; }
        public string? CustomerLastName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerCity { get; set; }
        public string? CustomerZIP { get; set; }
        public string? CustomerCountry { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CurrencyCode { get; set; }
        public string? PaymentPlan { get; set; }
    }
}
