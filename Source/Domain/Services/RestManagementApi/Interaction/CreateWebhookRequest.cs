using Newtonsoft.Json;

namespace BigCommerceApi.Domain.Services.RestManagementApi.Interaction
{
    public class CreateWebhookRequest
    {
        public string? Scope { get; set; }
        public string? Destination { get; set; }
        public object? Headers { get; set; }

        [JsonProperty("is_active")]
        public bool? IsActive { get; set; }
    }
}
