using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202409051017)]
    public class M202409051017_AddCSubjectPathToStoreConfiguration : Migration
    {
        public override void Up()
        {
            Alter.Table("StoreConfiguration")
                .AddColumn("SubjectPath").AsString(256).Nullable();
        }

        public override void Down()
        {
            Delete.Column("SubjectPath").FromTable("StoreConfiguration");
        }
    }
}
