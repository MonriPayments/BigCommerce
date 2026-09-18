using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202606091200)]
    public class M202606091200_AddPaymentGatewayAndPaymentConfigurationToStoreConfiguration : Migration
    {
        public override void Up()
        {
            Alter.Table("StoreConfiguration")
                .AddColumn("PaymentGateway").AsString(64).Nullable()
                .AddColumn("PaymentConfiguration").AsCustom("nvarchar(max)").Nullable();

            Execute.Sql("UPDATE [StoreConfiguration] SET [PaymentGateway] = 'WSPay' WHERE [PaymentGateway] IS NULL");
        }

        public override void Down()
        {
            Delete.Column("PaymentConfiguration").FromTable("StoreConfiguration");
            Delete.Column("PaymentGateway").FromTable("StoreConfiguration");
        }
    }
}
