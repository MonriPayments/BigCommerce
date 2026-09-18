using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
	[Migration(202403110947)]
	public class M202403110947_AddLanguageToShops : Migration
	{
		public override void Up()
		{
			Alter.Table("Shops")
				.AddColumn("Language").AsString(8).Nullable();
		}

		public override void Down()
		{
			Delete.Column("Shops")
				.FromTable("Language");
		}
	}
}
