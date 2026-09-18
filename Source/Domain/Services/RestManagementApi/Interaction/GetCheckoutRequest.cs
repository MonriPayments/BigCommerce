using RestSharp;

namespace BigCommerceApi.Domain.Services.RestManagementApi.Interaction
{
	public class GetCheckoutRequest
	{
		public Method? MethodType { get; set; }
		public string? AuthToken { get; set; }
		public string? MerchantApiPath { get; set; }
		public string? CheckoutId { get; set; }
	}
}
