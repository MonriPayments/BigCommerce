using BigCommerceApi.Domain.Services.RestManagementApi.Enums;
using Newtonsoft.Json;
using System;

namespace BigCommerceApi.Domain.Services.RestManagementApi.Interaction
{
    public class CreateScriptResponse : RestManagementApiResponse<CreateScriptResponse.ServiceResponse>
    {
        public CreateScriptResponse(ServiceResponse? serviceResponse = null, Exception? raisedException = null) : base(serviceResponse, raisedException)
        {

        }

        public class ServiceResponse
        {
            public Data? Data { get; set; }
            public object? Meta { get; set; }
            public string? Error { get; set; }
        }

        public class Data
        {
            public string? Name { get; set; }
            public string? Uuid { get; set; }
            public string? Description { get; set; }
            public string? Html { get; set; }
            public string? Src { get; set; }
            public Location? Location { get; set; }
            public Visibility? Visibility { get; set; }
            public Kind? Kind { get; set; }
            public bool? Enabled { get; set; }
            public int? ChannelId { get; set; }

            [JsonProperty("api_client_id")]
            public string? ApiClientId { get; set; }

            [JsonProperty("consent_category")]
            public ConsentCategory? ConsentCategory { get; set; }

            [JsonProperty("auto_uninstall")]
            public bool? AutoUninstall { get; set; }

            [JsonProperty("load_method")]
            public LoadMethod? LoadMethod { get; set; }

            [JsonProperty("date_created")]
            public string? DateCreated { get; set; }

            [JsonProperty("date_modified")]
            public string? DateModified { get; set; }
        }

    }
}
