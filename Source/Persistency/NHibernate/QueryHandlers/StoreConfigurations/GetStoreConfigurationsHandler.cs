using BigCommerceApi.Domain.Projections.StoreConfigurations;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
using BigCommerceApi.Domain.Services.StoreConfigurations.GetStoreConfigurations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Common.Contracts;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Domain.Services.StoreConfigurations.GetStoreConfigurations
{
    public class GetStoreConfigurationsHandler : BaseCommandHandler, IAsyncQueryHandler<GetStoreConfigurationsQuery, Response<GetStoreConfigurationsProjection>>
    {
        private readonly ILogger _logger;
        public GetStoreConfigurationsHandler(IQueryExecutor queryExecutor, UnitOfWork unitOfWork, ILogger logger) : base(queryExecutor, unitOfWork)
        {
            _logger = logger;
        }

        public async Task<Response<GetStoreConfigurationsProjection>> ExecuteAsync(GetStoreConfigurationsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var response = new Response<GetStoreConfigurationsProjection>();
                var storeConfigurations = await QueryExecutor.GetAllAsync<Model.Stores.StoreConfiguration>(cancellationToken);
                if (storeConfigurations == null)
                    return new Response<GetStoreConfigurationsProjection>(new GetStoreConfigurationsProjection { });
                var storeConfigurationList = storeConfigurations.Select(sc => new StoreConfigurationDto
                {
                    Id = sc.Id,
                    AccessToken = sc.AccessToken,
                    ClientName = sc.ClientName,
                    ClientID = sc.ClientID,
                    ClientSecret = sc.ClientSecret,
                    StoreApiAccountName = sc.StoreApiAccountName,
                    ApiPath = sc.ApiPath,
                    MerchantSiteUrl = sc.MerchantSiteUrl,
                    SubjectPath = sc.SubjectPath,
                    MerchantEmail = sc.MerchantEmail,
                    MerchantName = sc.MerchantName,
                    PaymentMethodName = sc.PaymentMethodName,
                    StoreStatus = sc.StoreStatus.ToString(),
                    CheckoutType = sc.CheckoutType.ToString(),
                    PaymentGateway = sc.PaymentGateway.ToString(),
                    PaymentConfiguration = sc.PaymentConfiguration,
                    OrderConfirmationRedirectRoute = sc.OrderConfirmationRedirectRoute,
                    DefaultOrderStatus = sc.DefaultOrderStatus.ToString(),
                    IdShop = sc.IdShop
                }).ToList();
                var result = new GetStoreConfigurationsProjection { StoreConfigurations = storeConfigurationList };
                return new Response<GetStoreConfigurationsProjection>(result);
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                    {
                        { "Handler", "GetStoreConfigurationsHandler" },
                    };
                _logger.LogApplicationError(ex, additionalData);
                return ResponseHelper.CreateErrorResponse<GetStoreConfigurationsProjection>(ErrorMessages.GenericError);
            }
        }
    }
}
