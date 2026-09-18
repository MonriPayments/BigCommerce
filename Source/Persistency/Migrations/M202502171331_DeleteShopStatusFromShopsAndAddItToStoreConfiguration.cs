using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202502171331)]
    public class M202502171331_DeleteShopStatusFromShopsAndAddItToStoreConfiguration : Migration
    {
        public override void Up()
        {
            Delete.Column("ShopStatus").FromTable("Shops");
            Alter.Table("StoreConfiguration")
                .AddColumn("StoreStatus").AsString(32).Nullable();
        }

        public override void Down()
        {
            Delete.Column("StoreStatus").FromTable("StoreConfiguration");
            Alter.Table("Shops")
                .AddColumn("ShopStatus").AsString(32).Nullable();
        }
    }
}
