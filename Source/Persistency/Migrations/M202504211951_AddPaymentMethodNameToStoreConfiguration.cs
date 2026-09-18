using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202504211951)]
    public class M202504211951_AddPaymentMethodNameToStoreConfiguration : Migration
    {
        public override void Up()
        {
            Alter.Table("StoreConfiguration")
                .AddColumn("PaymentMethodName").AsString(512).Nullable();
        }

        public override void Down()
        {
            Delete.Column("PaymentMethodName").FromTable("StoreConfiguration");
        }
    }
}
