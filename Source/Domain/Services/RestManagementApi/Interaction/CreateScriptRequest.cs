using Newtonsoft.Json;

namespace BigCommerceApi.Domain.Services.RestManagementApi.Interaction
{
    public class CreateScriptRequest
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string? Name { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string? Description { get; set; }

        [JsonProperty("html", NullValueHandling = NullValueHandling.Ignore)]
        public string? Html { get; set; }

        [JsonProperty("src", NullValueHandling = NullValueHandling.Ignore)]
        public string? Src { get; set; }

        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public string? Location { get; set; }

        [JsonProperty("visibility", NullValueHandling = NullValueHandling.Ignore)]
        public string? Visibility { get; set; }

        [JsonProperty("kind", NullValueHandling = NullValueHandling.Ignore)]
        public string? Kind { get; set; }

        [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Enabled { get; set; }

        [JsonProperty("auto_uninstall", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AutoUninstall { get; set; }

        [JsonProperty("load_method", NullValueHandling = NullValueHandling.Ignore)]
        public string? LoadMethod { get; set; }

        [JsonProperty("api_client_id", NullValueHandling = NullValueHandling.Ignore)]
        public string? ApiClientId { get; set; }

        [JsonProperty("consent_category", NullValueHandling = NullValueHandling.Ignore)]
        public string? ConsentCategory { get; set; }

        [JsonProperty("channel_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ChannelId { get; set; }
    }
}
