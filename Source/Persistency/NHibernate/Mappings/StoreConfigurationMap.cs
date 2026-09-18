using BigCommerceApi.Domain.Model.Stores;
using FluentNHibernate.Mapping;

namespace BigCommerceApi.Persistency.NHibernate.Mappings
{
    public class StoreConfigurationMap : ClassMap<StoreConfiguration>
	{
        public StoreConfigurationMap()
        {
            Id(_ => _.Id);
            Map(_ => _.AccessToken);
            Map(_ => _.ClientName);
            Map(_ => _.ClientID);
            Map(_ => _.ClientSecret);
            Map(_ => _.StoreApiAccountName);
            Map(_ => _.ApiPath);
            Map(_ => _.MerchantSiteUrl);
            Map(_ => _.SubjectPath);
            Map(_ => _.MerchantEmail);
            Map(_ => _.MerchantName);
            Map(_ => _.StoreStatus).CustomType<GenericEnumMapper<StoreStatus>>();
            Map(_ => _.CheckoutType).CustomType<GenericEnumMapper<CheckoutType>>();
            Map(_ => _.PaymentGateway).CustomType<GenericEnumMapper<PaymentGateway>>();
            Map(_ => _.PaymentConfiguration).CustomSqlType("nvarchar(max)");
            Map(_ => _.OrderConfirmationRedirectRoute);
            Map(_ => _.DefaultOrderStatus).CustomType<GenericEnumMapper<OrderStatus>>();
            Map(_ => _.PaymentMethodName);
        }
    }
}
