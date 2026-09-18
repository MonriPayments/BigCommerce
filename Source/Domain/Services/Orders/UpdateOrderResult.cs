namespace BigCommerceApi.Domain.Services.Orders
{
	public class UpdateOrderResult
	{
		public int? ActionSuccess { get; set; }
		public string? SiteUrl { get; set; }
		public string? MerchantName { get; set; }
		public string? MerchantEmail { get; set; }
		public string? Currency {  get; set; }
		public string? RedirectUrlRoute {  get; set; }
	}
}
