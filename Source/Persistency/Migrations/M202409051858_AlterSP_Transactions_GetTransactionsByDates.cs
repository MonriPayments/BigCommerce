using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202409051858)]
    public class M202409051858_AlterSP_Transactions_GetTransactionsByDates : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("M202409001858_AlterSP_Transactions_GetTransactionsByDates.sql");
        }

        public override void Down()
        {
            Execute.Sql("DROP PROCEDURE [dbo].[Transactions_GetTransactionsByDates]");
        }
    }
}
