using Newtonsoft.Json;
using System;

namespace BigCommerceApi.Domain.Services.RestManagementApi.Interaction
{
    public class CreateWebhookResponse : RestManagementApiResponse<CreateWebhookResponse.ServiceResponse>
    {
        public CreateWebhookResponse(ServiceResponse? serviceResponse = null, Exception? raisedException = null) : base(serviceResponse, raisedException)
        {

        }

        public sealed class ServiceResponse
        {
            public Data? Data { get; set; }
            public Meta? Meta { get; set; }
            public string? Error { get; set; }
        }

        public sealed class Data
        {
            public int? Id { get; set; }
            public string? Scope { get; set; }
            public string? Destination { get; set; }
            public object? Headers { get; set; }

            [JsonProperty("is_active")]
            public bool? IsActive { get; set; }

            [JsonProperty("client_id")]
            public string? ClientId { get; set; }

            [JsonProperty("store_hash")]
            public string? StoreHash { get; set; }

            [JsonProperty("created_at")]
            public int? CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public int? UpdatedAt { get; set; }
        }

        public sealed class Meta
        {
            public Pagination? Pagination { get; set; }
        }

        public sealed class Pagination
        {
            public int? Total { get; set; }
            public int? Count { get; set; }

            [JsonProperty("per_page")]
            public int? PerPage { get; set; }

            [JsonProperty("current_page")]
            public int? CurrentPage { get; set; }

            [JsonProperty("total_pages")]
            public int? TotalPages { get; set; }
            public Links? Links { get; set; }
        }

        public sealed class Links
        {
            public string? Previous { get; set; }
            public string? Current { get; set; }
            public string? Next { get; set; }
        }
    }
}
