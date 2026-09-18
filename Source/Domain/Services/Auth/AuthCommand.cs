using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Services.Auth
{
    public class AuthCommand : ICommand<Response<AuthResult>>
    {
        public string? AccountUuid { get; set; }
        public string? Code { get; set; }
        public string? Context { get; set; }
        public string? Scope { get; set; }
        public string? RequestId { get; set; }
    }
}
