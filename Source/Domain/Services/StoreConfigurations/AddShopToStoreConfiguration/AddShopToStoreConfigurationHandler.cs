using BigCommerceApi.Domain.Model.Shops;
using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
using BigCommerceApi.Domain.Services.Merchant;
using BigCommerceApi.Domain.Services.RestManagementApi;
using BigCommerceApi.Domain.Services.RestManagementApi.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Domain.Services.StoreConfigurations.AddShopToStoreConfiguration
{
    public class AddShopToStoreConfigurationHandler : BaseCommandHandler, IAsyncCommandHandler<AddShopToStoreConfigurationCommand, Response<AddShopToStoreConfigurationResult>>
    {
        private readonly ILogger _logger;

        public AddShopToStoreConfigurationHandler(IQueryExecutor queryExecutor, UnitOfWork unitOfWork, ILogger logger) : base(queryExecutor, unitOfWork)
        {
            _logger = logger;
        }

        public async Task<Response<AddShopToStoreConfigurationResult>> ExecuteAsync(AddShopToStoreConfigurationCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Command cannot be null");
            }
            if (string.IsNullOrEmpty(command.ShopID) || string.IsNullOrEmpty(command.SecretKey) || string.IsNullOrEmpty(command.StoreConfigurationId))
            {
                throw new ArgumentException("ShopID, SecretKey, and StoreConfigurationId cannot be null or empty");
            }

            //  string -->  Guid
            if (!Guid.TryParse(command.StoreConfigurationId, out var storeConfigurationIdGuid))
            {
                throw new ArgumentException("StoreConfigurationId must be a valid GUID");
            }

            var storeConfiguration = await QueryExecutor.GetOneAsync<StoreConfiguration>(_ => _.Id == storeConfigurationIdGuid, cancellationToken);

            if (storeConfiguration == null)
            {
                throw new InvalidOperationException($"StoreConfiguration with ID {storeConfigurationIdGuid} not found");
            }

            // Create a new Shop entity
            var shop = new BigCommerceApi.Domain.Model.Shops.Shops
            {
                ShopID = command.ShopID,
                SecretKey = command.SecretKey,
                Language = command.Language ?? "en",
                IsTokenShopId = command.IsTokenShopId ?? false,
                StoreConfiguration = storeConfiguration
            };

            try
            {
                UnitOfWork.Begin();
                UnitOfWork.RegisterEntityToAddOrUpdate(shop);
                await UnitOfWork.CommitAsync(cancellationToken);
                return new Response<AddShopToStoreConfigurationResult>
                {
                };
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "AddShopToStoreConfigurationHandler" },
                };
                _logger.LogApplicationError(ex, additionalData);
                return ResponseHelper.CreateErrorResponse<AddShopToStoreConfigurationResult>(ErrorMessages.GenericError);
            }
        }
    }
}
