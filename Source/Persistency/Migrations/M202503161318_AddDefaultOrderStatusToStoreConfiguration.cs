using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202503161318)]
    public class M202503161318_AddDefaultOrderStatusToStoreConfiguration : Migration
    {
        public override void Up()
        {
            Alter.Table("StoreConfiguration")
                .AddColumn("DefaultOrderStatus").AsString(128).Nullable();
        }

        public override void Down()
        {
            Delete.Column("DefaultOrderStatus").FromTable("StoreConfiguration");
        }
    }
}
