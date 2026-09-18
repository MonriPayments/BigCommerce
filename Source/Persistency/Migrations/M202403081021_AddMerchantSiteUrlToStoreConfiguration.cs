using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
	[Migration(202403081021)]
	public class M202403081021_AddMerchantSiteUrlToStoreConfiguration : Migration
	{
		public override void Up()
		{
			Alter.Table("StoreConfiguration")
				.AddColumn("MerchantSiteUrl").AsString(1000).Nullable();
		}

		public override void Down()
		{
			Delete.Column("MerchantSiteUrl")
				.FromTable("StoreConfiguration");
		}
	}
}
