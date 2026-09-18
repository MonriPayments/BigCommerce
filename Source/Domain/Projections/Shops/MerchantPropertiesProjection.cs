namespace BigCommerceApi.Domain.Projections.Shops
{
    public class MerchantPropertiesProjection
    {
        public string ShopIdOption { get; set; }
        public string MerchantName { get; set; }
        public string MerchantEmail { get; set; }
        public string MerchantSiteUrl { get; set; }
        public bool IsInProduction { get; set; }
        public string CheckoutType { get; set; }
        public string PaymentMethodName { get; set; }
    }
}
