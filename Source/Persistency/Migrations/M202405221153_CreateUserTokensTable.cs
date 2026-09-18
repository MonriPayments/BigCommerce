using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202405221153)]
    public class M202405221153_CreateUserTokensTable : Migration
    {
        public override void Up()
        {
            Create.Table("UserTokens")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("TokenCreated").AsDateTime().NotNullable()
                .WithColumn("Token").AsGuid().NotNullable()
                .WithColumn("TokenNumber").AsString(16).NotNullable()
                .WithColumn("PaymentType").AsString(64).Nullable()
                .WithColumn("CreditCardExpirationDate").AsDateTime().Nullable()
                .WithColumn("MaskedPan").AsString(64).Nullable()
                .WithColumn("IdUser").AsGuid().ForeignKey("FK_UserTokens_User", "User", "Id").NotNullable();
        }

        public override void Down()
        {
            Delete.Table("UserTokens");
        }
    }
}
