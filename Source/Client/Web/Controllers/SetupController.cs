using BigCommerceApi.Client.Web.Interaction.Requests;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Client.Web.Controllers
{
    [Route("setup")]
    public class SetupController : BaseController
    {
        public SetupController(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, ILogger logger) : base(queryExecutor, commandExecutor, logger)
        {
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }

        [HttpGet("merchantonboarding")]
        public IActionResult LoadMerchantOnboarding()
        {
            return RedirectToPage("/MerchantOnboarding");
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveBigCommerceMerchant(CancellationToken cancellationToken)
        {
            try
            {
                var body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                var request = JsonConvert.DeserializeObject<SaveMerchantAccount>(body)!;

                if (request == null)
                    return BadRequest();

                var saveMerchantAccountCommand = request.CreateDomainRequest();

                var domainResponse = await CommandExecutor.ExecuteAsync(saveMerchantAccountCommand, cancellationToken);

                return Ok();
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
