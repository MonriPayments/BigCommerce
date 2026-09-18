using Newtonsoft.Json;

namespace BigCommerceApi.Domain.Services.RestManagementApi.Interaction
{
	public class CheckoutCreateOrderRequest
	{

		[JsonProperty("status_id")]
		public int? StatusId { get; set; }
	}
}
