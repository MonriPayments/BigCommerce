using BigCommerceApi.Domain.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ILogger = WebStudio.Logging.Abstractions.ILogger;
using WebStudio.Common.Contracts;
using WebStudio.Entities.Interaction;
using BigCommerceApi.Domain.Services.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace BigCommerceApi.Client.Web.Pages
{
    public class LoginModel : PageModel
    {
        protected readonly ICommandExecutor CommandExecutor;
        protected readonly ILogger Logger;

        public LoginModel(ICommandExecutor commandExecutor, ILogger logger)
        {
            Argument.IsNotNull(commandExecutor, nameof(commandExecutor));
            Argument.IsNotNull(logger, nameof(logger));


            CommandExecutor = commandExecutor;
            Logger = logger;
        }

        [BindProperty]
        public string? Username { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostSubmit()
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Username!),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    //IsPersistent = true //for RememberMe func
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20) //Cookie expiration time
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToPage("/Success");
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "LoginUser" },
                };
                Logger.LogApplicationError(ex, additionalData);

                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages.GenericError);
            }
        }
    }
}
