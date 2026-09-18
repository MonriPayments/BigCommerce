using Newtonsoft.Json;
using System;

namespace BigCommerceApi.Domain.Services.RestManagementApi.Interaction
{
    public class GetAccessTokenResponse : RestManagementApiResponse<GetAccessTokenResponse.ServiceResponse>
    {
        public GetAccessTokenResponse(ServiceResponse? serviceResponse = null, Exception? raisedException = null) : base(serviceResponse, raisedException)
        {

        }

        public sealed class ServiceResponse
        {
            public User? User { get; set; }
            public Owner? Owner { get; set; }

            /// <summary>
            /// A space-separated list of the OAuth scopes this access_token authorizes access to
            /// </summary>
            public string? Scope { get; set; }

            /// <summary>
            /// The path that identifies the store in API requests to https://api.bigcommerce.com; a string of the form stores/{STORE_HASH}
            /// </summary>
            public string? Context { get; set; }

            /// <summary>
            /// The semi-permanent security token that your app can use to make requests on behalf of the store. Save this value securely for future requests
            /// </summary>
            [JsonProperty("access_token")]
            public string? AccessToken { get; set; }

            /// <summary>
            /// The ID of the Developer Portal account that registered the app profile
            /// </summary>
            [JsonProperty("account_uuid")]
            public string? AccountUuid { get; set; }

            public string? Error { get; set; }
        }

        public class User
        {
            /// <summary>
            /// BigCommerce’s unique identifier for the authorized user. Save this value to identify the user in future requests
            /// </summary>
            public int? Id { get; set; }

            /// <summary>
            /// The username that the authorized user has on file with BigCommerce
            /// </summary>
            public string? Username { get; set; }

            /// <summary>
            /// The email address that the authorized user has on file with BigCommerce. Save this value for future requests
            /// </summary>
            public string? Email { get; set; }
        }

        public class Owner
        {
            /// <summary>
            /// 	BigCommerce’s unique identifier for the store owner. Save this value to identify the user in future requests
            /// </summary>
            public int? Id { get; set; }

            /// <summary>
            /// The username that the store owner has on file with BigCommerce
            /// </summary>
            public string? Username { get; set; }

            /// <summary>
            /// The email address that the store owner has on file with BigCommerce. Save this value for future requests
            /// </summary>
            public string? Email { get; set; }
        }
    }
}