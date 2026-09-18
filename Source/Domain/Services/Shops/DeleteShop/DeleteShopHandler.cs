using BigCommerceApi.Domain.Model.Shops;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
using BigCommerceApi.Domain.Services.Shops.UpdateShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Domain.Services.Shops.DeleteShop
{
    public class DeleteShopHandler : BaseCommandHandler, IAsyncCommandHandler<DeleteShopCommand, Response<DeleteShopResult>>
    {
        private readonly ILogger _logger;

        public DeleteShopHandler(IQueryExecutor queryExecutor, UnitOfWork unitOfWork, ILogger logger) : base(queryExecutor, unitOfWork)
        {
            _logger = logger;
        }

        public async Task<Response<DeleteShopResult>> ExecuteAsync(DeleteShopCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Command cannot be null");
            }
            if (string.IsNullOrEmpty(command.Id))
            {
                throw new ArgumentException("Id cannot be null or empty", nameof(command.Id));
            }
            // string --> Guid
            if (!Guid.TryParse(command.Id, out var shopIdGuid))
            {
                throw new ArgumentException("ShopId must be a valid GUID", nameof(command.Id));
            }
            var shop = await QueryExecutor.GetOneAsync<BigCommerceApi.Domain.Model.Shops.Shops>(_ => _.Id == shopIdGuid, cancellationToken);
            if (shop == null)
            {
                throw new InvalidOperationException($"Shop with ID {shopIdGuid} not found");
            }
            try
            {
                UnitOfWork.Begin();
                UnitOfWork.RegisterEntityToDelete(shop);
                await UnitOfWork.CommitAsync(cancellationToken);
                return new Response<DeleteShopResult>
                {
                };
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "DeleteShopHandler" },
                };
                _logger.LogApplicationError(ex, additionalData);
                return ResponseHelper.CreateErrorResponse<DeleteShopResult>(ErrorMessages.GenericError);
            }
        }
    }
}
