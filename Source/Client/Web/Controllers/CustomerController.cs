using WebStudio.Entities.Interaction;
using Microsoft.AspNetCore.Mvc;
using BigCommerceApi.Client.Web.Extensions;
using BigCommerceApi.Domain.Services.Logging;
using ILogger = WebStudio.Logging.Abstractions.ILogger;
using BigCommerceApi.Domain.Services.RestManagementApi;
using WebStudio.Common.Contracts;
using BigCommerceApi.Client.Web.Interaction.Requests;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Client.Web.Interaction.Responses;
using BigCommerceApi.Domain.Services.CustomerTokens;

namespace BigCommerceApi.Client.Web.Controllers
{
    [Route("users")]
    public class CustomerController : BaseController
    {
        private readonly BigCommerceConfiguration _bigCommerceConfiguration;
        public CustomerController(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, ILogger logger, BigCommerceConfiguration bigCommerceConfiguration) : base(queryExecutor, commandExecutor, logger)
        {
            Argument.IsNotNull(bigCommerceConfiguration, nameof(bigCommerceConfiguration));

            _bigCommerceConfiguration = bigCommerceConfiguration;
        }

        [HttpGet("clientId")]
        public IActionResult? GetClientId()
        {
            return Ok (new GetClientIdResponse { ClientId = _bigCommerceConfiguration.AppClientID });
        }

        [HttpPost("customertokens")]
        public async Task<IActionResult> GetCustomerTokens([FromBody] GetBigCwtTokenRequest request, CancellationToken cancellationToken)
        {
            var requestId = Guid.NewGuid().ToString();
            try
            {
                Logger.LogAPICommunication(Events.BigCgetCustomerTokensRequestReceived,
                   new APIRequestLogState(nameof(GetCustomerTokens),
                   requestId,
                   request,
                   HttpContext.GetRequestHeaders(),
                   HttpContext.GetHttpMethod(),
                   HttpContext.GetEndpointPath()));

                if (request == null)
                    return BadRequest();

                var jwtTokenDecoded = JwtDecoder.DecodeJwt<CustomerTokenRequest>(request.Jwt!, _bigCommerceConfiguration.AppClientID!);

                if (jwtTokenDecoded == null)
                {
                    Logger.LogAPICommunication(Events.BigCgetCustomerTokensRequestReceived,
                       new APIRequestLogState(nameof(GetCustomerTokens),
                       requestId,
                       "Could not deserialize JWT Token",
                       HttpContext.GetRequestHeaders(),
                       HttpContext.GetHttpMethod(),
                       HttpContext.GetEndpointPath()));

                    CustomerTokensCommand customerTokensCommand = new CustomerTokensCommand
                    {
                        Email = String.Empty,
                        ApplicationId = String.Empty,
                        StoreHash = String.Empty
                    };

                    var response = await CommandExecutor.ExecuteAsync(customerTokensCommand, cancellationToken);

                    return Ok(response.Result);
                }

                var domainResponse = await CommandExecutor.ExecuteAsync(jwtTokenDecoded.CreateDomainRequest(), cancellationToken);

                if (domainResponse.HasErrors)
                {
                    Logger.LogDomainResponseError(new DomainResponseErrorLogState(nameof(GetCustomerTokens), requestId, domainResponse));
                    Logger.LogAPICommunication(Events.BigCgetCustomerTokensRequestReceived, new APIResponseLogState(nameof(GetCustomerTokens), requestId, domainResponse, 400));
                    return BadRequest(domainResponse);
                }

                return Ok(domainResponse.Result);
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Controller", "GetCustomerTokens" },
                    { "InnerException", ex.InnerException!.ToString() }
                };
                Logger.LogApplicationError(ex, additionalData);
                return StatusCode(StatusCodes.Status400BadRequest, ErrorMessages.GenericError);
            }
        }
    }
}
