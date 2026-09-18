namespace BigCommerceApi.Client.Web.Interaction.Requests
{
    public class UpdateShopRequest
    {
        public string? Id { get; set; }
        public string? ShopID { get; set; }
        public string? SecretKey { get; set; }
        public string? Language { get; set; }
        public bool? IsTokenShopId { get; set; }
    }
}
