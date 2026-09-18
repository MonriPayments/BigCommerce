namespace BigCommerceApi.Domain.Services.RestManagementApi.Enums
{
    public enum OrderStatuses
    {
        /// <summary>
        /// An incomplete order happens when a shopper reached the payment page, but did not complete the transaction.
        /// </summary>
        Incomplete = 0,
        /// <summary>
        /// Customer started the checkout process, but did not complete it.
        /// </summary>
        Pending = 1,
        /// <summary>
        /// Order has been shipped, but receipt has not been confirmed; seller has used the Ship Items action.
        /// </summary>
        Shipped = 2,
        /// <summary>
        /// Only some items in the order have been shipped, due to some products being pre-order only or other reasons.
        /// </summary>
        PartiallyShipped = 3,
        /// <summary>
        /// Seller has used the Refund action.
        /// </summary>
        Refunded = 4,
        /// <summary>
        /// Seller has cancelled an order, due to a stock inconsistency or other reasons.
        /// </summary>
        Cancelled = 5,
        /// <summary>
        /// Seller has marked the order as declined for lack of manual payment, or other reasons.
        /// </summary>
        Declined = 6,
        /// <summary>
        /// Customer has completed checkout process, but payment has yet to be confirmed.
        /// </summary>
        AwaitingPayment = 7,
        /// <summary>
        /// Order has been pulled, and is awaiting customer pickup from a seller-specified location.
        /// </summary>
        AwaitingPickup = 8,
        /// <summary>
        /// Order has been pulled and packaged, and is awaiting collection from a shipping provider.
        /// </summary>
        AwaitingShipment = 9,
        /// <summary>
        /// Client has paid for their digital product and their file(s) are available for download.
        /// </summary>
        Completed = 10,
        /// <summary>
        /// Customer has completed the checkout process and payment has been confirmed.
        /// </summary>
        AwaitingFulfullment = 11,
        /// <summary>
        /// Order is on hold while some aspect needs to be manually confirmed.
        /// </summary>
        ManualVerificationRequired = 12,
        /// <summary>
        /// Customer has initiated a dispute resolution process for the PayPal transaction that paid for the order.
        /// </summary>
        Disputed = 13,
        /// <summary>
        /// Seller has partially refunded the order.
        /// </summary>
        PartiallyRefunded = 14
    }
}
