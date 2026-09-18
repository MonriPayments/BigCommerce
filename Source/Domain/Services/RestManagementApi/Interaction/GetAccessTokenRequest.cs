using Newtonsoft.Json;

namespace BigCommerceApi.Domain.Services.RestManagementApi.Interaction
{
    public class GetAccessTokenRequest
    {
        /// <summary>
        /// Our app Client ID
        /// </summary>
        [JsonProperty("client_id")]
        public string? ClientId { get; set; }

        /// <summary>
        /// Our app Client Secret
        /// </summary>
        [JsonProperty("client_secret")]
        public string? ClientSecret { get; set; }

        /// <summary>
        /// Code from auth callback
        /// </summary>
        [JsonProperty("code")]
        public string? Code { get; set; }

        /// <summary>
        /// Store context from auth callback
        /// </summary>
        [JsonProperty("context")]
        public string? Context { get; set; }

        /// <summary>
        /// Store scope from auth callback
        /// </summary>
        [JsonProperty("scope")]
        public string? Scope { get; set; }

        /// <summary>
        /// The value is always "authorization_code"
        /// </summary>
        [JsonProperty("grant_type")]
        public string? GrantType { get; set; }

        /// <summary>
        /// Identical to the auth callback registered in the app profile
        /// </summary>
        [JsonProperty("redirect_uri")]
        public string? RedirectUri { get; set; }
    }
}
