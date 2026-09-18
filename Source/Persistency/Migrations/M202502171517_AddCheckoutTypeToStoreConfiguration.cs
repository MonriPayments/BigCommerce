using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202502171517)]
    public class M202502171517_AddCheckoutTypeToStoreConfiguration : Migration
    {
        public override void Up()
        {
            Alter.Table("StoreConfiguration")
                .AddColumn("CheckoutType").AsString(64).Nullable();
        }

        public override void Down()
        {
            Delete.Column("CheckoutType").FromTable("StoreConfiguration");
        }
    }
}
