using WebStudio.Entities.Interaction;
using Microsoft.AspNetCore.Mvc;
using ILogger = WebStudio.Logging.Abstractions.ILogger;
using BigCommerceApi.Client.Web.Extensions;
using BigCommerceApi.Domain.Services.Logging;
using Newtonsoft.Json;
using System.Text;
using BigCommerceApi.Client.Web.Interaction.Requests;

namespace BigCommerceApi.Client.Web.Controllers
{
    [Route("callback")]
    public class CallbackController : BaseController
    {
        public CallbackController(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, ILogger logger) : base(queryExecutor, commandExecutor, logger)
        {

        }

        [HttpPost]
        [Route("orderstatusupdate")]
        public async Task<IActionResult> OrderStatusUpdate(CancellationToken cancellationToken)
        {
            var requestId = new Guid().ToString();

            var rawRequestBody = await Request.GetRawBodyAsync();
            Logger.LogAPICommunication(Events.BigCUpdateOrderRequestReceived, new APIRequestLogState(nameof(OrderStatusUpdate), requestId, rawRequestBody, HttpContext.GetRequestHeaders(), HttpContext.GetHttpMethod(), HttpContext.GetEndpointPath()));

            OrderStatusUpdateRequest request = JsonConvert.DeserializeObject<OrderStatusUpdateRequest>(rawRequestBody)!;

            if (request == null)
                return BadRequest("Request is null!");

            Logger.LogAPICommunication(Events.BigCUpdateOrderRequestReceived, new APIRequestLogState(nameof(OrderStatusUpdate), requestId, request, HttpContext.GetRequestHeaders(), HttpContext.GetHttpMethod(), HttpContext.GetEndpointPath()));

            var command = request.CreateDomainRequest();
            command.RequestId = requestId;

            var domainResponse = await CommandExecutor.ExecuteAsync(command, cancellationToken);

            if (domainResponse.HasErrors)
            {
                Logger.LogDomainResponseError(new DomainResponseErrorLogState(nameof(OrderStatusUpdate), requestId, domainResponse));
                Logger.LogAPICommunication(Events.CallbackResponseSent, new APIResponseLogState(nameof(OrderStatusUpdate), requestId, domainResponse, 400));
                return BadRequest(domainResponse);
            }

            Logger.LogAPICommunication(Events.BigCUpdateOrderRequestSent, new APIResponseLogState(nameof(OrderStatusUpdate), requestId, domainResponse, 200));

            return Ok(domainResponse);
        }

        [HttpPost]
        [Route("savetransaction")]
        [Consumes("application/json")]
        public async Task<IActionResult> SaveTransaction(CancellationToken cancellationToken)
        {
            var requestId = new Guid().ToString();

            var rawRequestBody = await Request.GetRawBodyAsync();
            Logger.LogAPICommunication(Events.CallbackRequestReceived, new APIRequestLogState(nameof(SaveTransaction), requestId, rawRequestBody, HttpContext.GetRequestHeaders(), HttpContext.GetHttpMethod(), HttpContext.GetEndpointPath()));

            ProcessingTransactionCallbackRequest request = JsonConvert.DeserializeObject<ProcessingTransactionCallbackRequest>(rawRequestBody)!;

            if (request == null)
                return BadRequest("Request is null!");

            Logger.LogAPICommunication(Events.CallbackRequestReceived, new APIRequestLogState(nameof(SaveTransaction), requestId, request, HttpContext.GetRequestHeaders(), HttpContext.GetHttpMethod(), HttpContext.GetEndpointPath()));

            var domainResponse = await CommandExecutor.ExecuteAsync(request.CreateDomainRequest(), cancellationToken);

            if (domainResponse.HasErrors)
            {
                Logger.LogDomainResponseError(new DomainResponseErrorLogState(nameof(SaveTransaction), requestId, domainResponse));
                Logger.LogAPICommunication(Events.CallbackResponseSent, new APIResponseLogState(nameof(SaveTransaction), requestId, domainResponse, 400));
                return BadRequest(domainResponse);
            }

            Logger.LogAPICommunication(Events.CallbackResponseSent, new APIResponseLogState(nameof(SaveTransaction), requestId, domainResponse, 200));

            return Ok(domainResponse);
        }
    }

    public static class RequestExtensions
    {
        public static async Task<string> GetRawBodyAsync(
           this HttpRequest request, Encoding? encoding = null)
        {
            if (!request.Body.CanSeek)
                request.EnableBuffering();            

            request.Body.Position = 0;
            var reader = new StreamReader(request.Body, encoding ?? Encoding.UTF8);
            var body = await reader.ReadToEndAsync().ConfigureAwait(false);
            request.Body.Position = 0;
            return body;
        }

    }
}
