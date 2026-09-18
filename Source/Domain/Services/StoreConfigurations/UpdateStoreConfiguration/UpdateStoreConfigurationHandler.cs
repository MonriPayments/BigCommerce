using BigCommerceApi.Domain.Model.Shops;
using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
using BigCommerceApi.Domain.Services.StoreConfigurations.AddShopToStoreConfiguration;
using BigCommerceApi.Domain.Services.StoreConfigurations.EditStoreConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Domain.Services.StoreConfigurations.UpdateStoreConfiguration
{
    public class UpdateStoreConfigurationHandler : BaseCommandHandler, IAsyncCommandHandler<UpdateStoreConfigurationCommand, Response<UpdateStoreConfigurationResult>>
    {
        private readonly ILogger _logger;

        public UpdateStoreConfigurationHandler(IQueryExecutor queryExecutor, UnitOfWork unitOfWork, ILogger logger) : base(queryExecutor, unitOfWork)
        {
            _logger = logger;
        }
        public async Task<Response<UpdateStoreConfigurationResult>> ExecuteAsync(UpdateStoreConfigurationCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Command cannot be null");
            }
            if (string.IsNullOrEmpty(command.Id))
            {
                throw new ArgumentException("Id cannot be null or empty", nameof(command.Id));
            }
            //  string -->  Guid
            if (!Guid.TryParse(command.Id, out var storeConfigurationIdGuid))
            {
                throw new ArgumentException("StoreConfigurationId must be a valid GUID");
            }

            var storeConfiguration = await QueryExecutor.GetOneAsync<StoreConfiguration>(_ => _.Id == storeConfigurationIdGuid, cancellationToken);
            if (storeConfiguration == null)
            {
                throw new InvalidOperationException($"StoreConfiguration with ID {storeConfigurationIdGuid} not found");
            }

            // Update the store configuration properties
            storeConfiguration.AccessToken = command.AccessToken ?? storeConfiguration.AccessToken;
            storeConfiguration.ClientName = command.ClientName ?? storeConfiguration.ClientName;
            storeConfiguration.ClientID = command.ClientID ?? storeConfiguration.ClientID;
            storeConfiguration.ClientSecret = command.ClientSecret ?? storeConfiguration.ClientSecret;
            storeConfiguration.StoreApiAccountName = command.StoreApiAccountName ?? storeConfiguration.StoreApiAccountName;
            storeConfiguration.ApiPath = command.ApiPath ?? storeConfiguration.ApiPath;
            storeConfiguration.MerchantSiteUrl = command.MerchantSiteUrl ?? storeConfiguration.MerchantSiteUrl;
            storeConfiguration.SubjectPath = command.SubjectPath ?? storeConfiguration.SubjectPath;
            storeConfiguration.MerchantEmail = command.MerchantEmail ?? storeConfiguration.MerchantEmail;
            storeConfiguration.MerchantName = command.MerchantName ?? storeConfiguration.MerchantName;
            storeConfiguration.PaymentMethodName = command.PaymentMethodName ?? storeConfiguration.PaymentMethodName;
            storeConfiguration.StoreStatus = command.StoreStatus ?? storeConfiguration.StoreStatus;
            storeConfiguration.CheckoutType = command.CheckoutType ?? storeConfiguration.CheckoutType;
            storeConfiguration.PaymentGateway = command.PaymentGateway ?? storeConfiguration.PaymentGateway;
            storeConfiguration.PaymentConfiguration = command.PaymentConfiguration ?? storeConfiguration.PaymentConfiguration;
            storeConfiguration.OrderConfirmationRedirectRoute = command.OrderConfirmationRedirectRoute ?? storeConfiguration.OrderConfirmationRedirectRoute;
            storeConfiguration.DefaultOrderStatus = command.DefaultOrderStatus ?? storeConfiguration.DefaultOrderStatus;
            storeConfiguration.IdShop = command.IdShop ?? storeConfiguration.IdShop;

            // Save changes to the database
            try
            {
                UnitOfWork.Begin();
                UnitOfWork.RegisterEntityToAddOrUpdate(storeConfiguration);
                await UnitOfWork.CommitAsync(cancellationToken);
                return new Response<UpdateStoreConfigurationResult>
                {
                };
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "UpdateStoreConfigurationHandler" },
                };
                _logger.LogApplicationError(ex, additionalData);
                return ResponseHelper.CreateErrorResponse<UpdateStoreConfigurationResult>(ErrorMessages.GenericError);
            }
        }
    }
}
