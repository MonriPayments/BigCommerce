using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202502171037)]
    public class M202502171037_AddMerchantSiteAndEmailToStoreConfiguration : Migration
    {
        public override void Up()
        {
            Alter.Table("StoreConfiguration")
                .AddColumn("MerchantEmail").AsString(256).Nullable()
                .AddColumn("MerchantName").AsString(256).Nullable();
        }

        public override void Down()
        {
            Delete.Column("MerchantEmail").FromTable("StoreConfiguration");
            Delete.Column("MerchantName").FromTable("StoreConfiguration");
        }
    }
}
