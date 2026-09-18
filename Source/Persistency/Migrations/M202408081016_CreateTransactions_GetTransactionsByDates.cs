using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202408081016)]
    public class M202408081016_CreateTransactions_GetTransactionsByDates : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("M202408081016_CreateTransactions_GetTransactionsByDates.sql");
        }

        public override void Down()
        {
            Execute.Sql("DROP PROCEDURE [dbo].[Transactions_GetTransactionsByDates]");
        }
    }
}
