using BigCommerceApi.Domain.Model.Users;
using FluentNHibernate.Mapping;

namespace BigCommerceApi.Persistency.NHibernate.Mappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(_ => _.Id);

            Map(_ => _.Email);
            References(_ => _.StoreConfiguration).Column("IdStoreConfiguration").Not.LazyLoad();
        }
    }
}
