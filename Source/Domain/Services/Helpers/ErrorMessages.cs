namespace BigCommerceApi.Domain.Services.Helpers
{
    public static class ErrorMessages
    {
        public const string StoreConfigurationNotFound = "An error occurred: Store configuration not found.";
        public const string ShopIdNotFound = "An error occurred: ShopId not found.";
        public const string CheckoutProcessFailed = "Failed to process checkout into an order. Please verify checkout details and try again.";
        public const string WSPayFormTransactionNotCreated = "Failed to create transaciton on WSPay form. Please verify request.";
        public const string GenericError = "An unexpected error occurred. Please try again later.";
        public const string NumericCodeNotFound = "Numeric code not found.";
        public const string AlphabeticCodeNotFound = "Alphabetic code not found.";
        public const string MerchantAccountCreationFailed = "An error ocurred during merchant creation";
        public const string TransactionNotFound = "Transaction not found";
        public const string FailedToGenerateAccessToken = "Error upon creation of access token";
    }
}
