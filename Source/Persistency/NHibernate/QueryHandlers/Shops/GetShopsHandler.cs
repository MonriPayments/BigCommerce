using BigCommerceApi.Domain.Projections.Shops;
using BigCommerceApi.Domain.Services;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
using BigCommerceApi.Domain.Services.StoreConfigurations.GetStoreConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Persistency.NHibernate.QueryHandlers.Shops
{
    public class GetShopsHandler : BaseCommandHandler, IAsyncQueryHandler<GetShopsQuery, List<GetShopProjection>>
    {
        private readonly ILogger _logger;
        public GetShopsHandler(IQueryExecutor queryExecutor, UnitOfWork unitOfWork, ILogger logger) : base(queryExecutor, unitOfWork)
        {
            _logger = logger;
        }

        public Task<List<GetShopProjection>> ExecuteAsync(GetShopsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var shops = QueryExecutor.GetAllAsync<Domain.Model.Shops.Shops>(cancellationToken).Result;
                if (shops == null)
                    return Task.FromResult(new List<GetShopProjection>());

                // Convert string to Guid
                if (!Guid.TryParse(query.StoreConfigurationId, out var storeConfigurationGuid))
                {
                    _logger.LogApplicationError(new ArgumentException("Invalid StoreConfigurationId format"), new Dictionary<string, string>
                    {
                        { "Handler", "GetShopsHandler" },
                        { "StoreConfigurationId", query.StoreConfigurationId }
                    });
                    return Task.FromResult(new List<GetShopProjection>());
                }

                var filteredShops = shops
                    .Where(s => s.StoreConfiguration != null &&
                                s.StoreConfiguration.Id == storeConfigurationGuid)
                    .ToList();

                var shopProjections = filteredShops.Select(s => new GetShopProjection
                {
                    Id = s.Id,
                    ShopID = s.ShopID,
                    SecretKey = s.SecretKey,
                    Language = s.Language,
                    IsTokenShopId = s.IsTokenShopId
                }).ToList();
                return Task.FromResult(shopProjections);
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "GetShopsHandler" },
                };
                _logger.LogApplicationError(ex, additionalData);
                return Task.FromResult(new List<GetShopProjection>());
            }
        }
    }
}
