using BigCommerceApi.Domain.Services.CustomerTokens;
using Newtonsoft.Json;

namespace BigCommerceApi.Client.Web.Interaction.Requests
{
    public class CustomerTokenRequest : BaseAPIRequest<CustomerTokensCommand>
    {
        public Customer? Customer { get; set; }
        public string? ApplicationId { get; set; }
        public string? StoreHash { get; set; }

        protected override void PopulateDomainRequest(CustomerTokensCommand domainRequest)
        {
            domainRequest.ApplicationId = this.ApplicationId;
            domainRequest.StoreHash = this.StoreHash;
            domainRequest.Email = this.Customer?.Email;
        }
    }

    public class Customer
    {
        public string Id { get; set; }
        public string? Email { get; set; }

        [JsonProperty("group_id")]
        public string? GroupId { get; set; }
    }
}
