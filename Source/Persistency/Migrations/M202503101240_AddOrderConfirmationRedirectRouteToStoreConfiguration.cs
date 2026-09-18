using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202503101240)]
    public class M202503101240_AddOrderConfirmationRedirectRouteToStoreConfiguration : Migration
    {
        public override void Up()
        {
            Alter.Table("StoreConfiguration")
                .AddColumn("OrderConfirmationRedirectRoute").AsString(128).Nullable();
        }

        public override void Down()
        {
            Delete.Column("OrderConfirmationRedirectRoute").FromTable("StoreConfiguration");
        }
    }
}
