using BigCommerceApi.Domain.Services.Cache;
using WebStudio.Entities.Interaction;
using Microsoft.AspNetCore.Mvc;
using BigCommerceApi.Client.Web.Interaction.Responses;
using BigCommerceApi.Client.Web.Extensions;
using BigCommerceApi.Domain.Services.Logging;
using BigCommerceApi.Client.Web.Pages;
using BigCommerceApi.Domain.Services.Helpers;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Client.Web.Controllers
{
    [Route("error")]
    public class ErrorController : BaseController
    {
        private readonly IWebAppCache _memoryCache;
        public ErrorController(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, ILogger logger, IWebAppCache memoryCache) : base(queryExecutor, commandExecutor, logger)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet("errorpage")]
        public async Task<IActionResult> ErrorPage([FromQuery] ReturnUrlWsPayFormResponse query, CancellationToken cancellationToken)
        {
            if (query == null)
                return BadRequest();

            Logger.LogAPICommunication(Events.WSPayAPIResponseReceived,
                new APIRequestLogState(nameof(Index),
                $"{query.ShoppingCartId}",
                query,
                HttpContext.GetRequestHeaders(),
                HttpContext.GetHttpMethod(),
                HttpContext.GetEndpointPath()));

            var updateCommand = query.CreateDomainResponse();

            var cachedValues = new CacheStore();
            if (_memoryCache.TryGetValue(updateCommand.OrderId, out var cachedData))
            {
                cachedValues = cachedData as CacheStore;
            }

            var errorModel = new ErrorModel();
            errorModel.SiteUrl = cachedValues.SiteUrl;
            errorModel.Lang = query.Lang;

            return RedirectToPage("/Error", new { siteUrl = errorModel.SiteUrl, lang = errorModel.Lang });
        }
    }
}
