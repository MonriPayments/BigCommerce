using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BigCommerceApi.Client.Web.Interaction.Requests
{
    public class LoadRequest
    {
        [FromQuery(Name = "signed_payload_jwt")]
        [JsonProperty("signed_payload_jwt")]
        public string? SignedPayloadJwt { get; set; }
    }

    public class LoadRequestJwt
    {
        /// <summary>
        /// The app API account's client ID; the intended audience
        /// </summary>
        [JsonProperty("aud")]
        public string? Aud { get; set; }

        /// <summary>
        /// The issuer; the value is always bc
        /// </summary>
        [JsonProperty("iss")]
        public string? Iss { get; set; }

        /// <summary>
        /// The issued at time; when the JWT was issued
        /// </summary>
        [JsonProperty("iat")]
        public string? Iat { get; set; }

        /// <summary>
        /// The not valid before time; when the JWT became or becomes valid. The value is always the same as iat
        /// </summary>
        [JsonProperty("nbf")]
        public string? Nbf { get; set; }

        /// <summary>
        /// The expiration time; when the JWT becomes invalid. Currently, 24 hours after nbf
        /// </summary>
        [JsonProperty("exp")]
        public string? Exp { get; set; }

        /// <summary>
        /// The JWT ID; a unique identifier for the JWT
        /// </summary>
        [JsonProperty("jti")]
        public string? Jti { get; set; }

        /// <summary>
        /// Identifies the subject store in API requests to https://api.bigcommerce.com; a string of the form stores/{STORE_HASH}
        /// </summary>
        [JsonProperty("sub")]
        public string? Sub { get; set; }

        /// <summary>
        /// Also known as a deep link. A developer-configured path that provides the app more information about the resource that initiated the load callback
        /// </summary>
        [JsonProperty("url")]
        public string? Url { get; set; }

        /// <summary>
        /// The channel associated with the click event that dispatched the callback. The value is null when a click in the Apps menu sidebar initiates the load callback
        /// </summary>
        [JsonProperty("channel_id")]
        public string? ChannelId { get; set; }

        [JsonProperty("user")]
        public User? User { get; set; }

        [JsonProperty("owner")]
        public Owner? Owner { get; set; }
    }

    public class User
    {
        /// <summary>
        /// The ID of the store owner
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; set; }

        /// <summary>
        /// The email address of the store owner
        /// </summary>
        [JsonProperty("email")]
        public string? Email { get; set; }

        /// <summary>
        /// The BCP 47 language tag of the store owner
        /// </summary>
        [JsonProperty("locale")]
        public string? Locale { get; set; }
    }

    public class Owner
    {
        /// <summary>
        /// The ID of the store owner
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; set; }

        /// <summary>
        /// The email address of the store owner
        /// </summary>
        [JsonProperty("email")]
        public string? Email { get; set; }
    }
}
