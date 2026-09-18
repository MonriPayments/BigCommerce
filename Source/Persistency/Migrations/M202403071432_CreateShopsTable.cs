using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
	[Migration(202403071432)]
	public class M202403071432_CreateShopsTable : Migration
	{
		public override void Up()
		{
			Create.Table("Shops")
				.WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
				.WithColumn("ShopID").AsString(512).NotNullable()
				.WithColumn("SecretKey").AsString(512).NotNullable()
				.WithColumn("IsTokenShopId").AsBoolean().NotNullable()
                .WithColumn("IdStoreConfiguration").AsGuid().ForeignKey("FK_StoreConfiguration_Shops", "StoreConfiguration", "Id").NotNullable();
        }

		public override void Down()
		{
			Delete.Table("Shops");
		}

	}
}
