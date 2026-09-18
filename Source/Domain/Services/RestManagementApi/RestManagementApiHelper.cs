using BigCommerceApi.Domain.Services.Auth;
using BigCommerceApi.Domain.Services.RestManagementApi.Enums;
using BigCommerceApi.Domain.Services.RestManagementApi.Interaction;

namespace BigCommerceApi.Domain.Services.RestManagementApi
{
    public static class RestManagementApiHelper
    {
        public const string GrantType = "authorization_code";
        public const string Scope = "\"store/order/statusUpdated\"";

        public static CreateScriptRequest CreateScriptRequest(string scriptName, string scriptDesc, string scriptSrc, Location location)
        {
            return new CreateScriptRequest
            {
                Name = scriptName,
                Description = scriptDesc,
                Kind = Kind.src.ToString(),
                AutoUninstall = true,
                LoadMethod = LoadMethod.defer.ToString(),
                Location = location.ToString(),
                Visibility = Visibility.checkout.ToString(),
                ConsentCategory = ConsentCategory.essential.ToString(),
                Enabled = true,
                Src = scriptSrc
            };
        }

        public static CreateWebhookRequest CreateWebhookRequest(string appUri)
        {
            return new CreateWebhookRequest
            {
                Scope = Scope,
                Destination = $"{appUri}callback/orderstatusupdate",
                Headers = null,
                IsActive = true,
            };
        }

        public static GetAccessTokenRequest GetAccessTokenRequest(AuthCommand command, BigCommerceConfiguration bigCommerceConfiguration)
        {
            return new GetAccessTokenRequest
            {
                ClientId = bigCommerceConfiguration.AppClientID,
                ClientSecret = bigCommerceConfiguration.AppClientSecret,
                Code = command.Code,
                Scope = command.Scope,
                Context = command.Context,
                RedirectUri = bigCommerceConfiguration.AppRedirectUri,
                GrantType = GrantType
            };
        }
    }
}
