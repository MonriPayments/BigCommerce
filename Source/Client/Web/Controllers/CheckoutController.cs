using BigCommerceApi.Client.Web.Extensions;
using BigCommerceApi.Client.Web.Interaction.Requests;
using BigCommerceApi.Client.Web.Interaction.Responses;
using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Services.Checkout.GetIFrameValues;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Client.Web.Controllers
{
    [Route("checkout")]
    public class CheckoutController : BaseController
    {
        public CheckoutController(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, ILogger logger) : base(queryExecutor, commandExecutor, logger)
        {
        }

        [HttpPost("init")]
        public async Task<IActionResult> InitiateCheckout(CancellationToken cancellationToken)
        {
            try
            {
                var body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                var request = JsonConvert.DeserializeObject<InitiateCheckoutRequest>(body)!;

                if (request == null)
                    return BadRequest();

                Logger.LogAPICommunication(Events.InitiateCheckoutRequestReceived,
                   new APIRequestLogState(nameof(InitiateCheckout),
                   $"{request.CheckoutID}",
                   request,
                   HttpContext.GetRequestHeaders(),
                   HttpContext.GetHttpMethod(),
                   HttpContext.GetEndpointPath()));

                var initCommand = request.CreateDomainRequest();

                var domainResponse = await CommandExecutor.ExecuteAsync(initCommand, cancellationToken);

                Logger.LogAPICommunication(Events.InitiateCheckoutResponseSent,
                   new APIRequestLogState(nameof(InitiateCheckout),
                   $"{request.CheckoutID}",
                   domainResponse,
                   HttpContext.GetRequestHeaders(),
                   HttpContext.GetHttpMethod(),
                   HttpContext.GetEndpointPath()));

                return Ok(new InitiateCheckoutResponse { Url = domainResponse.Result?.FormUrl! });
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "InitiateCheckoutHandler" },
                };
                Logger.LogApplicationError(ex, additionalData);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages.GenericError);
            }
        }

        [HttpPost("check-merchant-integration")]
        public async Task<string> CheckMerchantIntegration(CancellationToken cancellationToken)
        {
            var body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<CheckMerchantIntegrationRequest>(body)!;

            if (request == null)
                return CheckoutType.EmbeddedCheckout.ToString();

            var storeConfiguratiton = await QueryExecutor.GetOneAsync<StoreConfiguration>(_ => _.MerchantSiteUrl == request.SiteUrl);

            if (storeConfiguratiton != null)
                return storeConfiguratiton.CheckoutType.ToString();

            return CheckoutType.EmbeddedCheckout.ToString();
        }

        [HttpPost("fetch-payment-method-name")]
        public async Task<string> FetchPaymentMethodName(CancellationToken cancellationToken)
        {
            const string DEFAULT_PAYMENT_METHOD_NAME = "Plaćanje karticama";
            var body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<CheckMerchantIntegrationRequest>(body)!;

            if (request == null)
                return DEFAULT_PAYMENT_METHOD_NAME;

            var storeConfiguratiton = await QueryExecutor.GetOneAsync<StoreConfiguration>(_ => _.MerchantSiteUrl == request.SiteUrl);

            if (storeConfiguratiton != null)
                return storeConfiguratiton.PaymentMethodName!;

            return DEFAULT_PAYMENT_METHOD_NAME;
        }

        [HttpPost("get-iframe-values")]
        public async Task<IActionResult> GetIframeValues(CancellationToken cancellationToken)
        {
            try
            {
                var body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                var request = JsonConvert.DeserializeObject<GetIFrameValuesRequest>(body)!;

                if (request == null)
                    return BadRequest();

                Logger.LogAPICommunication(Events.GetIframeValuesRequest,
                   new APIRequestLogState(nameof(GetIframeValues),
                   $"{request.CheckoutID}",
                   request,
                   HttpContext.GetRequestHeaders(),
                   HttpContext.GetHttpMethod(),
                   HttpContext.GetEndpointPath()));

                var domainResponse = await CommandExecutor.ExecuteAsync(request.CreateDomainRequest(), cancellationToken);

                Logger.LogAPICommunication(Events.GetIframeValuesResponse,
                   new APIRequestLogState(nameof(GetIframeValues),
                   $"{request.CheckoutID}",
                   domainResponse,
                   HttpContext.GetRequestHeaders(),
                   HttpContext.GetHttpMethod(),
                   HttpContext.GetEndpointPath()));

                return Ok(new List<GetIFrameValuesResult>(domainResponse.Result!));
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Controller", "GetIframeValues" },
                };
                Logger.LogApplicationError(ex, additionalData);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages.GenericError);
            }
        }
    }
}
