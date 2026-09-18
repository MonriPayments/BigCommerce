using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
	[Migration(202403071434)]
	public class M202403071434_CreateStoreConfigurationTable : Migration
	{
		public override void Up()
		{
			Create.Table("StoreConfiguration")
				.WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
				.WithColumn("AccessToken").AsString(512).NotNullable()
				.WithColumn("ClientName").AsString(512).NotNullable()
				.WithColumn("ClientID").AsString(512).NotNullable()
				.WithColumn("ClientSecret").AsString(512).NotNullable()
				.WithColumn("StoreApiAccountName").AsString(512).NotNullable()
				.WithColumn("ApiPath").AsString(512).NotNullable()
				.WithColumn("IdShop").AsGuid().ForeignKey("FK_Shops_StoreConfiguration", "Shops", "Id").NotNullable();
		}

		public override void Down()
		{
			Delete.Table("StoreConfiguration");
		}

	}
}
