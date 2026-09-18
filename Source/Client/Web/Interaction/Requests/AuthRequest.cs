using BigCommerceApi.Domain.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BigCommerceApi.Client.Web.Interaction.Requests
{
    public class AuthRequest : BaseAPIRequest<AuthCommand>
    {
        [FromQuery(Name = "account_uuid")]
        [JsonProperty("account_uuid")]
        public string? AccountUuid { get; set; }

        [FromQuery(Name = "code")]
        [JsonProperty("code")]
        public string? Code { get; set; }

        [FromQuery(Name = "context")]
        [JsonProperty("context")]
        public string? Context {  get; set; }

        [FromQuery(Name = "scope")]
        [JsonProperty("scope")]
        public string? Scope { get; set; }

        protected override void PopulateDomainRequest(AuthCommand domainRequest)
        {
            domainRequest.AccountUuid = AccountUuid;
            domainRequest.Code = Code;
            domainRequest.Context = Context;
            domainRequest.Scope = Scope;
        }

        public bool IsValid()
        {
            return AccountUuid != null && Code != null && Scope != null && Context != null;
        }
    }
}
