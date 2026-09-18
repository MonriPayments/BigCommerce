using BigCommerceApi.Client.Web.Interaction.Requests;
using BigCommerceApi.Domain.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebStudio.Common.Contracts;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;
using BigCommerceApi.Domain.Services.Logging;

namespace BigCommerceApi.Client.Web.Pages
{
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class MerchantOnboarding : PageModel
    {
        protected readonly ICommandExecutor CommandExecutor;
        protected readonly ILogger Logger;
        public MerchantOnboarding(ICommandExecutor commandExecutor, ILogger logger)
        {
            Argument.IsNotNull(commandExecutor, nameof(commandExecutor));
            Argument.IsNotNull(logger, nameof(logger));


            CommandExecutor = commandExecutor;
            Logger = logger;
        }

        [BindProperty]
        public string? ShopId { get; set; }

        [BindProperty]
        public string? SecretKey { get; set; }

        [BindProperty]
        public string? AccessToken { get; set; }

        [BindProperty]
        public string? ClientName { get; set; }

        [BindProperty]
        public string? ClientId { get; set; }

        [BindProperty]
        public string? ClientSecret { get; set; }

        [BindProperty]
        public string? Name { get; set; }

        [BindProperty]
        public string? ApiPath { get; set; }

        [BindProperty]
        public string? MerchantSiteUrl { get; set; }

        public async Task<IActionResult> OnPostSubmit()
        {
            try
            {
                SaveMerchantAccount saveMerchantAccount = new SaveMerchantAccount();

                saveMerchantAccount.AccessToken = this.AccessToken;
                saveMerchantAccount.ClientName = this.ClientName;
                saveMerchantAccount.ClientSecret = this.ClientSecret;
                saveMerchantAccount.Name = this.Name;
                saveMerchantAccount.MerchantSiteUrl = this.MerchantSiteUrl;
                saveMerchantAccount.SecretKey = this.SecretKey;
                saveMerchantAccount.ApiPath = this.ApiPath;
                saveMerchantAccount.ShopId = this.ShopId;
                saveMerchantAccount.ClientId = this.ClientId;

                var domainResponse = await CommandExecutor.ExecuteAsync(saveMerchantAccount.CreateDomainRequest());
                
                if(domainResponse.HasErrors)
                {
                    return RedirectToPage("/Error");
                }

                return RedirectToPage("/Success");
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "SaveBigCommerceMerchant" },
                };
                Logger.LogApplicationError(ex, additionalData);

                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages.GenericError);
            }

        }

    }
}
