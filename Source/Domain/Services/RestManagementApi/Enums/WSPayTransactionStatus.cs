namespace BigCommerceApi.Domain.Services.RestManagementApi.Enums
{
    public enum WSPayTransactionStatus
    {
        NoChange = 0,
        Authorized = 1,
        Completed,
        Refunded,
        Voided,
        PartiallyRefunded
    }
}
