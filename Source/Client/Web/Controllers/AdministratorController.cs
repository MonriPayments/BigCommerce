using BigCommerceApi.Client.Web.Interaction.Requests;
using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Projections.Auth;
using BigCommerceApi.Domain.Projections.Logs;
using BigCommerceApi.Domain.Projections.Shops;
using BigCommerceApi.Domain.Projections.StoreConfigurations;
using BigCommerceApi.Domain.Services.Jwt;
using BigCommerceApi.Domain.Services.Shops.DeleteShop;
using BigCommerceApi.Domain.Services.Shops.UpdateShop;
using BigCommerceApi.Domain.Services.StoreConfigurations.AddShopToStoreConfiguration;
using BigCommerceApi.Domain.Services.StoreConfigurations.EditStoreConfiguration;
using BigCommerceApi.Domain.Services.StoreConfigurations.GetStoreConfigurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Remotion.Linq;
using System.Threading;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;
using IQueryExecutor = WebStudio.Entities.Interaction.IQueryExecutor;

namespace BigCommerceApi.Client.Web.Controllers
{
    [Route("administrator")]
    public class AdministratorController : BaseController
    {
        private readonly ILogger _logger;

        public AdministratorController(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, ILogger logger) : base(queryExecutor, commandExecutor, logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return BadRequest("Request is null.");
            }
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email and Password are required.");
            }
            var loginQuery = new LoginQuery
            {
                Email = request.Email,
                Password = request.Password
            };
            var domainResponse = await QueryExecutor.ExecuteAsync(loginQuery, cancellationToken);
            if (domainResponse.HasErrors)
            {
                return BadRequest(domainResponse);
            }
            return Ok(new { Token = domainResponse.Result?.Token });
        }

        [HttpGet("storeConfigurations")]
        [Authorize]
        public async Task<IActionResult> GetStoreConfigurationsAsync(CancellationToken cancellationToken)
        {
            var domainResponse = await QueryExecutor.ExecuteAsync(new GetStoreConfigurationsQuery(), cancellationToken);
            return Ok(domainResponse);
        }

        [HttpGet("shops/{storeConfigId}")]
        [Authorize]
        public async Task<IActionResult> GetShopsByConfigurationStoreAsync(CancellationToken cancellationToken, [FromRoute] string storeConfigId)
        {
            var domainResponse = await QueryExecutor.ExecuteAsync(new GetShopsQuery(storeConfigId), cancellationToken);
            return Ok(domainResponse);
        }

        [HttpPost]
        [Route("shops")]
        [Authorize]
        public async Task<IActionResult> AddShopToStoreConfigurationAsync([FromBody] AddShopToStoreConfigurationRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return BadRequest("Request is null.");
            }
            var command = new AddShopToStoreConfigurationCommand
            {
                ShopID = request.ShopID,
                SecretKey = request.SecretKey,
                Language = request.Language,
                IsTokenShopId = request.IsTokenShopId ?? false,
                StoreConfigurationId = request.StoreConfigurationId
            };
            var domainResponse = await CommandExecutor.ExecuteAsync(command, cancellationToken);
            if (domainResponse.HasErrors)
            {
                return BadRequest(domainResponse);
            }
            return Ok(domainResponse);
        }

        [HttpPut]
        [Route("storeConfigurations")]
        [Authorize]
        public async Task<IActionResult> UpdateStoreConfigurationAsync([FromBody] UpdateStoreConfigurationRequest storeConfiguration, CancellationToken cancellationToken)
        {
            if (storeConfiguration == null)
            {
                return BadRequest("Store configuration is null.");
            }
            var command = new UpdateStoreConfigurationCommand
            {
                Id = storeConfiguration.Id,
                AccessToken = storeConfiguration.AccessToken,
                ClientName = storeConfiguration.ClientName,
                ClientID = storeConfiguration.ClientID,
                ClientSecret = storeConfiguration.ClientSecret,
                StoreApiAccountName = storeConfiguration.StoreApiAccountName,
                ApiPath = storeConfiguration.ApiPath,
                MerchantSiteUrl = storeConfiguration.MerchantSiteUrl,
                SubjectPath = storeConfiguration.SubjectPath,
                MerchantEmail = storeConfiguration.MerchantEmail,
                MerchantName = storeConfiguration.MerchantName,
                PaymentMethodName = storeConfiguration.PaymentMethodName,
                StoreStatus = Enum.TryParse<StoreStatus>(storeConfiguration.StoreStatus, out var parsedStoreStatus) ? parsedStoreStatus : (StoreStatus?)null,
                CheckoutType = Enum.TryParse<CheckoutType>(storeConfiguration.CheckoutType, out var parsedCheckoutType) ? parsedCheckoutType : (CheckoutType?)null,
                PaymentGateway = Enum.TryParse<PaymentGateway>(storeConfiguration.PaymentGateway, out var parsedPaymentGateway) ? parsedPaymentGateway : (PaymentGateway?)null,
                PaymentConfiguration = storeConfiguration.PaymentConfiguration,
                OrderConfirmationRedirectRoute = storeConfiguration.OrderConfirmationRedirectRoute,
                DefaultOrderStatus = Enum.TryParse<OrderStatus>(storeConfiguration.DefaultOrderStatus, out var parsedOrderStatus) ? parsedOrderStatus : (OrderStatus?)null,
                IdShop = storeConfiguration.IdShop
            };
            var domainResponse = await CommandExecutor.ExecuteAsync(command, cancellationToken);
            if (domainResponse.HasErrors)
            {
                return BadRequest(domainResponse);
            }
            return Ok(domainResponse);
        }

        [HttpPut]
        [Route("shops")]
        [Authorize]
        public async Task<IActionResult> UpdateShopAsync([FromBody] UpdateShopRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return BadRequest("Request is null.");
            }
            var command = new UpdateShopCommand
            {
                Id = request.Id,
                ShopID = request.ShopID,
                SecretKey = request.SecretKey,
                Language = request.Language,
                IsTokenShopId = request.IsTokenShopId ?? false
            };
            var domainResponse = await CommandExecutor.ExecuteAsync(command, cancellationToken);
            if (domainResponse.HasErrors)
            {
                return BadRequest(domainResponse);
            }
            return Ok(domainResponse);
        }

        [HttpDelete]
        [Route("shops/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteShopAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id cannot be null or empty.");
            }
            var command = new DeleteShopCommand { Id = id };
            var domainResponse = await CommandExecutor.ExecuteAsync(command, cancellationToken);
            if (domainResponse.HasErrors)
            {
                return BadRequest(domainResponse);
            }
            return Ok(domainResponse);
        }

        [HttpPost]
        [Route("getLogs")]
        [Authorize]
        public async Task<IActionResult> GetLogsAsync([FromBody] GetLogsRequest request, CancellationToken cancellationToken)
        {
            if (!DateTimeOffset.TryParse(request.FromDate, out var fromDate))
            {
                return BadRequest("Invalid FromDate format.");
            }

            if (!DateTimeOffset.TryParse(request.ToDate, out var toDate))
            {
                return BadRequest("Invalid ToDate format.");
            }

            var domainResponse = await QueryExecutor.ExecuteAsync(new GetLogsQuery(fromDate,toDate), cancellationToken);

            return Ok(domainResponse);
        }
    }
    }
