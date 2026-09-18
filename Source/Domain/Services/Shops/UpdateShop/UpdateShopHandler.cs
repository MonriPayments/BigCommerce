using BigCommerceApi.Domain.Model.Shops;
using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
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

namespace BigCommerceApi.Domain.Services.Shops.UpdateShop
{
    public class UpdateShopHandler : BaseCommandHandler, IAsyncCommandHandler<UpdateShopCommand, Response<UpdateShopResult>>
    {
        private readonly ILogger _logger;

        public UpdateShopHandler(IQueryExecutor queryExecutor, UnitOfWork unitOfWork, ILogger logger) : base(queryExecutor, unitOfWork)
        {
            _logger = logger;
        }

        public async Task<Response<UpdateShopResult>> ExecuteAsync(UpdateShopCommand command, CancellationToken cancellationToken)
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
            if (!Guid.TryParse(command.Id, out var shopIdGuid))
            {
                throw new ArgumentException("ShopId must be a valid GUID");
            }
            var shop = await QueryExecutor.GetOneAsync<BigCommerceApi.Domain.Model.Shops.Shops>(_ => _.Id == shopIdGuid, cancellationToken);
            if (shop == null)
            {
                throw new InvalidOperationException($"Shop with ID {shopIdGuid} not found");
            }
            // Update the shop properties
            shop.ShopID = command.ShopID ?? shop.ShopID;
            shop.SecretKey = command.SecretKey ?? shop.SecretKey;
            shop.Language = command.Language ?? shop.Language;
            shop.IsTokenShopId = command.IsTokenShopId ?? shop.IsTokenShopId;

            try
            {
                UnitOfWork.Begin();
                UnitOfWork.RegisterEntityToAddOrUpdate(shop);
                await UnitOfWork.CommitAsync(cancellationToken);
                return new Response<UpdateShopResult>
                {
                };
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "UpdateShopHandler" },
                };
                _logger.LogApplicationError(ex, additionalData);
                return ResponseHelper.CreateErrorResponse<UpdateShopResult>(ErrorMessages.GenericError);
            }
        }

    }
}
