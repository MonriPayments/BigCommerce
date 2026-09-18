using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Projections.Shops;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Entities.Interaction;
using WebStudio.Persistency.NHibernateCore.Interaction;

namespace BigCommerceApi.Persistency.NHibernate.QueryHandlers.Shops
{
    public sealed class GetMerchantOptionsHandler : BaseQueryHandler, IAsyncQueryHandler<GetMerchantPropertiesQuery, List<MerchantPropertiesProjection>>
    {
        public GetMerchantOptionsHandler(ISession session) : base(session) { }

        public Task<List<MerchantPropertiesProjection>> ExecuteAsync (GetMerchantPropertiesQuery query, CancellationToken cancellationToken)
        {
            var shops = Session.Query<BigCommerceApi.Domain.Model.Shops.Shops>();

            shops = shops.Where(_ => _.StoreConfiguration.SubjectPath == query.SubjectPath);

            var merchantProperties = new List<MerchantPropertiesProjection>();

            foreach (var shop in shops)
            {
                var merchantProperty = new MerchantPropertiesProjection
                {
                    ShopIdOption = shop.ShopID,
                    MerchantEmail = shop.StoreConfiguration!.MerchantEmail,
                    MerchantName = shop.StoreConfiguration!.MerchantName,
                    MerchantSiteUrl = shop.StoreConfiguration!.MerchantSiteUrl,
                    IsInProduction = StoreStatus.Production == shop.StoreConfiguration!.StoreStatus,
                    CheckoutType = shop.StoreConfiguration!.CheckoutType.ToString(),
                    PaymentMethodName = shop.StoreConfiguration!.PaymentMethodName
                };

                merchantProperties.Add(merchantProperty);
            }

            return Task.FromResult(merchantProperties.ToList());
        }
    }
}
