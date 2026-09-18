using Newtonsoft.Json;

namespace BigCommerceApi.Domain.Services.RestManagementApi.Interaction
{
    public class UpdateOrderRequest
	{
		[JsonProperty("status_id")]
		public int? StatusId { get; set; }

        [JsonProperty("payment_method")]
        public string? PaymentMethod { get; set; }

        [JsonProperty("refunded_amount", NullValueHandling = NullValueHandling.Ignore)]
        public string? RefundedAmount { get; set; }

        [JsonProperty("total_inc_tax", NullValueHandling = NullValueHandling.Ignore)]
        public string? TotalIncTax { get; set; }

        [JsonProperty("total_ex_tax", NullValueHandling = NullValueHandling.Ignore)]
        public string? TotalExTax { get; set; }
	}
}
