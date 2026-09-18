using BigCommerceApi.Client.Web.Extensions;
using BigCommerceApi.Client.Web.Interaction.Responses;
using BigCommerceApi.Client.Web.Pages;
using BigCommerceApi.Domain.Services.Cache;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
using Microsoft.AspNetCore.Mvc;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Client.Web.Controllers
{
    [Route("redirect")]
    public class RedirectController : BaseController
    {
        private readonly IWebAppCache _memoryCache;
        public RedirectController(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, ILogger logger, IWebAppCache memoryCache) : base(queryExecutor, commandExecutor, logger)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet("order-confirmation")]
        public async Task<IActionResult> RedirectToOrderConfirmation([FromQuery] ReturnUrlWsPayFormResponse query, CancellationToken cancellationToken)
        {
            if (query == null)
                return BadRequest();

            Logger.LogAPICommunication(Events.WSPayAPIResponseReceived,
                new APIRequestLogState(nameof(RedirectToOrderConfirmation),
                $"{query.ShoppingCartId}",
                query,
                HttpContext.GetRequestHeaders(),
                HttpContext.GetHttpMethod(),
                HttpContext.GetEndpointPath()));

            var updateCommand = query.CreateDomainResponse();

            var domainResponse = await CommandExecutor.ExecuteAsync(updateCommand, cancellationToken);

            if(domainResponse.HasErrors)
            {
                var cachedValues = new CacheStore();
                if (_memoryCache.TryGetValue(updateCommand.OrderId, out var cachedData))
                {
                    cachedValues = cachedData as CacheStore;
                }

                var errorModel = new ErrorModel();
                errorModel.SiteUrl = cachedValues.SiteUrl;
                errorModel.Lang = query.Lang;

                Logger.LogAPICommunication(Events.DomainResponseErrorOccured,
                    new APIRequestLogState(nameof(RedirectToOrderConfirmation),
                    $"{query.ShoppingCartId}",
                    domainResponse,
                    HttpContext.GetRequestHeaders(),
                    HttpContext.GetHttpMethod(),
                    HttpContext.GetEndpointPath()));

                
                return RedirectToPage("/Error", new { siteUrl = errorModel.SiteUrl, lang = errorModel.Lang });
            }

            Logger.LogAPICommunication(Events.WSPayAPIResponseReceived,
                new APIRequestLogState(nameof(RedirectToOrderConfirmation),
                $"{query.ShoppingCartId}",
                domainResponse,
                HttpContext.GetRequestHeaders(),
                HttpContext.GetHttpMethod(),
                HttpContext.GetEndpointPath()));

            var model = query.PopulateOrderConfirmationModel();

            model.MerchantSiteUrl = domainResponse?.Result!.SiteUrl;
            model.CurrencyCode = CurrencyCodeHelper.getCurrencySign(CurrencyCodeHelper.getNumericCode(domainResponse?.Result!.Currency!));
            model.MerchantName = domainResponse?.Result!.MerchantName;
            model.MerchantEmail = domainResponse?.Result!.MerchantEmail;
            model.ShowMerchantInfo = !string.IsNullOrWhiteSpace(domainResponse?.Result!.MerchantName) && !string.IsNullOrWhiteSpace(domainResponse?.Result!.MerchantEmail);

            //special IF for skolska knjiga, God save us all.
            if (!string.IsNullOrWhiteSpace(domainResponse?.Result?.RedirectUrlRoute) && domainResponse?.Result?.RedirectUrlRoute == "/OrderConfirmationPostMessage")
            {
                return RedirectToPage("/OrderConfirmationPostMessage");
            }

            if (!string.IsNullOrWhiteSpace(domainResponse?.Result?.RedirectUrlRoute) && domainResponse?.Result?.RedirectUrlRoute != "/OrderConfirmation")
                return RedirectPermanent($"{domainResponse?.Result!.SiteUrl}{domainResponse?.Result?.RedirectUrlRoute}");

            return RedirectToPage("/OrderConfirmation", model);
        }

    }
}
