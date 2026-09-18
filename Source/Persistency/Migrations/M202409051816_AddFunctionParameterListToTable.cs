using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202409051816)]
    public class M202409051816_AddFunctionParameterListToTable : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("M202409001816_CreateFunction_ParameterListToTable");
        }

        public override void Down()
        {
            Execute.Sql("DROP FUNCTION [dbo].[ParameterListToTable]");
        }
    }
}
