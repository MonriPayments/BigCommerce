using BigCommerceApi.Domain.Model.Shops;
using FluentNHibernate.Mapping;

namespace BigCommerceApi.Persistency.NHibernate.Mappings
{
    public class ShopsMap : ClassMap<Shops>
	{
        public ShopsMap()
        {
            Id(_ => _.Id);

            Map(_ => _.ShopID);
            Map(_ => _.SecretKey);
            Map(_ => _.Language);
            Map(_ => _.IsTokenShopId);

            References(_ => _.StoreConfiguration).Column("IdStoreConfiguration").Not.LazyLoad(); 
        }
    }
}
