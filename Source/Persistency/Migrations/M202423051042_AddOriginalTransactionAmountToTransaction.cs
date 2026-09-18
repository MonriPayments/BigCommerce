using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202423051042)]
    public class M202423051042_AddOriginalTransactionAmountToTransaction : Migration
    {
        public override void Up()
        {
            Alter.Table("Transaction")
                .AddColumn("OriginalTransactionAmount").AsDouble().Nullable();
        }

        public override void Down()
        {
            Delete.Column("OriginalTransactionAmount").FromTable("Transaction");
        }
    }
}
