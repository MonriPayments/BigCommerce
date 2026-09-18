using FluentMigrator;

namespace BigCommerceApi.Persistency.Migrations
{
    [Migration(202404291116)]
    public class M202404291116_CreateTransactionTable : Migration
    {
        public override void Up()
        {
            Create.Table("Transaction")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("WsPayOrderId").AsString(255).NotNullable()
                .WithColumn("UniqueTransactionNumber").AsInt64().NotNullable()
                .WithColumn("Signature").AsString(255).Nullable()
                .WithColumn("STAN").AsString(50).Nullable()
                .WithColumn("ApprovalCode").AsString(255).Nullable()
                .WithColumn("ErrorMessage").AsString(1000).Nullable()
                .WithColumn("ShopID").AsString(20).Indexed().NotNullable()
                .WithColumn("ShoppingCartID").AsString(255).Indexed().NotNullable()
                .WithColumn("Amount").AsDouble().NotNullable()
                .WithColumn("CurrencyCode").AsInt32().Nullable()
                .WithColumn("Success").AsBoolean().Nullable()
                .WithColumn("Authorized").AsBoolean().Nullable()
                .WithColumn("Completed").AsBoolean().Nullable()
                .WithColumn("Voided").AsBoolean().Nullable()
                .WithColumn("Refunded").AsBoolean().Nullable()
                .WithColumn("PaymentPlan").AsString(4).Nullable()
                .WithColumn("Partner").AsString(255).Nullable()
                .WithColumn("OnSite").AsString(255).Nullable()
                .WithColumn("CreditCardName").AsString(64).Indexed().NotNullable()
                .WithColumn("CreditCardNumber").AsString(20).Indexed().NotNullable()
                .WithColumn("ECI").AsString(16).Nullable()
                .WithColumn("CustomerFirstName").AsString(255).Indexed().NotNullable()
                .WithColumn("CustomerLastName").AsString(255).Indexed().NotNullable()
                .WithColumn("CustomerAddress").AsString(255).Nullable()
                .WithColumn("CustomerCity").AsString(50).Nullable()
                .WithColumn("CustomerCountry").AsString(255).Nullable()
                .WithColumn("CustomerPhone").AsString(50).Nullable()
                .WithColumn("CustomerZIP").AsString(50).Nullable()
                .WithColumn("CustomerEmail").AsString(255).Nullable()
                .WithColumn("TransactionDateTime").AsDateTime().Indexed().NotNullable()
                .WithColumn("Token").AsString(255).Nullable()
                .WithColumn("TokenNumber").AsString(255).Nullable()
                .WithColumn("ExpirationDate").AsString(50).Nullable();
        }

        public override void Down()
        {
            Delete.Table("Transaction");
        }
    }
}
