using BigCommerceApi.Domain.Model.Users;
using FluentNHibernate.Mapping;

namespace BigCommerceApi.Persistency.NHibernate.Mappings
{
    public class UserTokensMap : ClassMap<UserTokens>
    {
        public UserTokensMap()
        {
            Id(_ => _.Id);

            Map(_ => _.TokenCreated);
            Map(_ => _.Token);
            Map(_ => _.TokenNumber);
            Map(_ => _.PaymentType);
            Map(_ => _.CreditCardExpirationDate);
            Map(_ => _.MaskedPan);

            References(_ => _.User).Column("IdUser").Not.LazyLoad();
        }
    }
}
