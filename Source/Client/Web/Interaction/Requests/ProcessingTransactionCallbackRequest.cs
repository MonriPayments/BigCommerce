using BigCommerceApi.Domain.Services.Callback.SaveTransaction;

namespace BigCommerceApi.Client.Web.Interaction.Requests
{
    public class ProcessingTransactionCallbackRequest : BaseAPIRequest<SaveCallbackCommand>
    {
        public ProcessingTransactionCallbackRequest()
        {

        }

        public string? WsPayOrderId { get; set; }
        public int? UniqueTransactionNumber { get; set; }
        public string? Signature { get; set; }
        public string? STAN { get; set; }
        public string? ApprovalCode { get; set; }
        public string? ShopID { get; set; }
        public string? ShoppingCartID { get; set; }
        public double? Amount { get; set; }
        public int? CurrencyCode { get; set; }
        public string? ActionSuccess { get; set; }
        public string? Authorized { get; set; }
        public string? Completed { get; set; }
        public string? Voided { get; set; }
        public string? Refunded { get; set; }
        public string? PaymentPlan { get; set; }
        public string? Partner { get; set; }
        public string? OnSite { get; set; }
        public string? CreditCardName { get; set; }
        public string? CreditCardNumber { get; set; }
        public string? ECI { get; set; }
        public string? CustomerFirstName { get; set; }
        public string? CustomerLastName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerCity { get; set; }
        public string? CustomerCountry { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerZIP { get; set; }
        public string? CustomerEmail { get; set; }
        public bool? IsLessThen30DaysFromTransaction { get; set; }
        public bool? CanBeCompleted { get; set; }
        public bool? CanBeVoided { get; set; }
        public bool? CanBeRefunded { get; set; }
        public string? TransactionDateTime { get; set; }
        public string? Token { get; set; }
        public string? TokenNumber { get; set; }
        public string? ExpirationDate { get; set; }

        protected override void PopulateDomainRequest(SaveCallbackCommand domainRequest)
        {
            domainRequest.WsPayOrderId = WsPayOrderId;
            domainRequest.UniqueTransactionNumber = UniqueTransactionNumber;
            domainRequest.Signature = Signature;
            domainRequest.STAN = STAN;
            domainRequest.ApprovalCode = ApprovalCode;
            domainRequest.ShopID = ShopID;
            domainRequest.ShoppingCartID = ShoppingCartID;
            domainRequest.Amount = Amount;
            domainRequest.CurrencyCode = CurrencyCode;
            domainRequest.Success = ActionSuccess;
            domainRequest.Authorized = Authorized;
            domainRequest.Completed = Completed;
            domainRequest.Voided = Voided;
            domainRequest.Refunded = Refunded;
            domainRequest.PaymentPlan = PaymentPlan;
            domainRequest.Partner = Partner;
            domainRequest.OnSite = OnSite;
            domainRequest.CreditCardName = CreditCardName;
            domainRequest.CreditCardNumber = CreditCardNumber;
            domainRequest.ECI = ECI;
            domainRequest.CustomerFirstName = CustomerFirstName;
            domainRequest.CustomerLastName = CustomerLastName;
            domainRequest.CustomerAddress = CustomerAddress;
            domainRequest.CustomerCity = CustomerCity;
            domainRequest.CustomerCountry = CustomerCountry;
            domainRequest.CustomerPhone = CustomerPhone;
            domainRequest.CustomerZIP = CustomerZIP;
            domainRequest.CustomerEmail = CustomerEmail;
            domainRequest.TransactionDateTime = TransactionDateTime;
            domainRequest.Token = Token;
            domainRequest.TokenNumber = TokenNumber;
            domainRequest.ExpirationDate = ExpirationDate;
        }
    }
}
