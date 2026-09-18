
using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Services.CustomerTokens
{
    public class CustomerTokensCommand : ICommand<Response<CustomerTokensResult>>
    {
        public string? Email { get; set; }
        public string? ApplicationId { get; set; }
        public string? StoreHash { get; set; }
    }
}
