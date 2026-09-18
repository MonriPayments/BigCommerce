using BigCommerceApi.Domain.Model.Stores;
using WebStudio.Entities.Core;

namespace BigCommerceApi.Domain.Model.Shops
{
    public sealed class Shops : Entity 
	{
		public string? ShopID { get; set; }
		public string? SecretKey { get; set; }
		public string? Language {  get; set; }
		public bool? IsTokenShopId {  get; set; }
        public StoreConfiguration? StoreConfiguration { get; set; }
    }
}
