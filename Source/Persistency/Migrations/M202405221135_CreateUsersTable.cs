using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202405221135)]
    public class M202405221135_CreateUsersTable : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Email").AsString(512).NotNullable()
                .WithColumn("IdStoreConfiguration").AsGuid().ForeignKey("FK_User_StoreConfiguration", "StoreConfiguration", "Id").NotNullable();
        }

        public override void Down()
        {
            Delete.Table("User");
        }
    }
}
