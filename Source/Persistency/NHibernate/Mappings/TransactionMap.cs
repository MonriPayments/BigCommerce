using BigCommerceApi.Domain.Model.Transactions;
using FluentNHibernate.Mapping;

namespace BigCommerceApi.Persistency.NHibernate.Mappings
{
    public class TransactionMap : ClassMap<Transaction>
    {
        public TransactionMap()
        {
            Id(_ => _.Id);
            Map(_ => _.WsPayOrderId);
            Map(_ => _.UniqueTransactionNumber);
            Map(_ => _.Signature);
            Map(_ => _.STAN);
            Map(_ => _.ApprovalCode);
            Map(_ => _.ErrorMessage);
            Map(_ => _.ShopID);
            Map(_ => _.ShoppingCartID);
            Map(_ => _.Amount);
            Map(_ => _.CurrencyCode);
            Map(_ => _.Success);
            Map(_ => _.Authorized);
            Map(_ => _.Completed);
            Map(_ => _.Voided);
            Map(_ => _.Refunded);
            Map(_ => _.PaymentPlan);
            Map(_ => _.Partner);
            Map(_ => _.OnSite);
            Map(_ => _.CreditCardName);
            Map(_ => _.CreditCardNumber);
            Map(_ => _.ECI);
            Map(_ => _.CustomerFirstName);
            Map(_ => _.CustomerLastName);
            Map(_ => _.CustomerAddress);
            Map(_ => _.CustomerCity);
            Map(_ => _.CustomerCountry);
            Map(_ => _.CustomerPhone);
            Map(_ => _.CustomerZIP);
            Map(_ => _.CustomerEmail);
            Map(_ => _.TransactionDateTime);
            Map(_ => _.Token);
            Map(_ => _.TokenNumber);
            Map(_ => _.ExpirationDate);
            Map(_ => _.OriginalTransactionAmount);
        }
    }
}
