using BigCommerceApi.Domain.Projections.Transactions;
using FluentNHibernate.Mapping;

namespace BigCommerceApi.Persistency.NHibernate.Mappings
{
    public class TransactionProjectionMap : ClassMap<TransactionProjection>
    {
        public TransactionProjectionMap()
        {
            Not.LazyLoad();
            ReadOnly();
            SchemaAction.None();
            Polymorphism.Explicit();

            Id(_ => _.WsPayOrderId);
            Map(_ => _.UniqueTransactionNumber);
            Map(_ => _.CreditCardName);
            Map(_ => _.Amount);
            Map(_ => _.CurrencyCode);
            Map(_ => _.TransactionDateTime);
            Map(_ => _.ShopID);
            Map(_ => _.CustomerFirstName);
            Map(_ => _.CustomerLastName);
            Map(_ => _.Authorized);
            Map(_ => _.Completed);
            Map(_ => _.Voided);
            Map(_ => _.Refunded);
            Map(_ => _.ShoppingCartID);
        }
    }
}
