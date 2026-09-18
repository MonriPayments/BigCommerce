using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202403281342)]
    public class M202403281342_AddShopStatusToShops : Migration
    {
        public override void Up()
        {
            Alter.Table("Shops")
                .AddColumn("ShopStatus").AsString(32).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Shops")
                .FromTable("ShopStatus");
        }
    }
}
