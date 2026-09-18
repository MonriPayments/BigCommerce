namespace BigCommerceApi.Domain.Services.WSPayForm.Interaction
{
	public class CreateFormTransactionRequest
	{
		public string? ShopID { get; set; }
		public string? ShoppingCartID { get; set; }
		public string? Version { get; set; } = "2.0";
		public string? TotalAmount { get; set; }
		public string? ReturnURL { get; set; }
		public string? ReturnErrorURL { get; set; }
		public string? CancelURL { get; set; }
		public string? Signature { get; set; }
		public string? PaymentPlan { get; set; }
		public string? TokenNumber { get; set; }
		public string? Token { get; set; }
		public string? Lang { get; set; }
		public string? CustomerFirstName { get; set; }
		public string? CustomerLastName { get; set; }
		public string? CustomerAddress { get; set; }
		public string? CustomerCity { get; set; }
		public string? CustomerZIP { get; set; }
		public string? CustomerCountry { get; set; }
		public string? CustomerEmail { get; set; }
		public string? CustomerPhone { get; set; }
		public string? CreditCardName { get; set; }
		public string? PaymentMethod { get; set; }
		public string? IntAmount { get; set; }
		public string? IntCurrency { get; set; }
		public string? IsTokenRequest { get; set; }
		public string? IframeResponseTarget { get; set; }
		public string? iFrame { get; set; }
		public string? ReturnMethod { get; set; }
		public string? CurrencyCode { get; set; }
	}
}
