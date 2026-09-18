using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202507160947)]
    public class M202507160947_CreateAdministratorTable : Migration
    {
        public override void Up()
        {
            Create.Table("Administrator")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Password").AsString(512).NotNullable()
                .WithColumn("Email").AsString(512).NotNullable()
                .WithColumn("LastLogin").AsDateTime().Nullable();
        }
        public override void Down()
        {
            Delete.Table("Administrator");
        }
    }
}
