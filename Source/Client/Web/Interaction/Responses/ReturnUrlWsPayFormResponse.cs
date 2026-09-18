using BigCommerceApi.Client.Web.Pages;
using BigCommerceApi.Domain.Services.Orders;

namespace BigCommerceApi.Client.Web.Interaction.Responses
{
	public class ReturnUrlWsPayFormResponse : BaseAPIResponse<UpdateOrderCommand>
	{
		/// <summary>
		/// Gets or sets the customer's first name.
		/// </summary>
		public string? CustomerFirstName { get; set; }

		/// <summary>
		/// Gets or sets the customer's last name.
		/// </summary>
		public string? CustomerSurname { get; set; }

		/// <summary>
		/// Gets or sets the customer's address.
		/// </summary>
		public string? CustomerAddress { get; set; }

		/// <summary>
		/// Gets or sets the customer's city.
		/// </summary>
		public string? CustomerCity { get; set; }

		/// <summary>
		/// Gets or sets the customer's ZIP code.
		/// </summary>
		public string? CustomerZIP { get; set; }

		/// <summary>
		/// Gets or sets the customer's country.
		/// </summary>
		public string? CustomerCountry { get; set; }

		/// <summary>
		/// Gets or sets the customer's phone number.
		/// </summary>
		public string? CustomerPhone { get; set; }

		/// <summary>
		/// Gets or sets the customer's email address.
		/// </summary>
		public string? CustomerEmail { get; set; }

		/// <summary>
		/// Gets or sets the shopping cart ID.
		/// </summary>
		public string? ShoppingCartId { get; set; }

		/// <summary>
		/// Gets or sets the language.
		/// </summary>
		public string? Lang { get; set; }

		/// <summary>
		/// Gets or sets the date and time.
		/// </summary>
		public DateTime dateTime { get; set; }

		/// <summary>
		/// Gets or sets the amount.
		/// </summary>
		public string? Amount { get; set; }

		/// <summary>
		/// Gets or sets the ECI (Electronic Commerce Indicator).
		/// </summary>
		public string? ECI { get; set; }

		/// <summary>
		/// Gets or sets the STAN.
		/// </summary>
		public string? STAN { get; set; }

		/// <summary>
		/// Gets or sets the partner.
		/// </summary>
		public string? Partner { get; set; }

		/// <summary>
		/// Gets or sets the WS Pay order ID.
		/// </summary>
		public string? WsPayOrderId { get; set; }

		/// <summary>
		/// Gets or sets the payment type.
		/// </summary>
		public string? PaymentType { get; set; }

		/// <summary>
		/// Gets or sets the credit card number.
		/// </summary>
		public string? CreditCardNumber { get; set; }

		/// <summary>
		/// Gets or sets the payment plan.
		/// </summary>
		public string? PaymentPlan { get; set; }

		/// <summary>
		/// Gets or sets the shop posted payment plan.
		/// </summary>
		public string? ShopPostedPaymentPlan { get; set; }

		/// <summary>
		/// Gets or sets the shop posted language.
		/// </summary>
		public string? ShopPostedLang { get; set; }

		/// <summary>
		/// Gets or sets the shop posted credit card name.
		/// </summary>
		public string? ShopPostedCreditCardName { get; set; }

		/// <summary>
		/// Gets or sets the success indicator.
		/// </summary>
		public int Success { get; set; }

		/// <summary>
		/// Gets or sets the approval code.
		/// </summary>
		public string? ApprovalCode { get; set; }

		/// <summary>
		/// Gets or sets the error message.
		/// </summary>
		public string? ErrorMessage { get; set; }

		/// <summary>
		/// Gets or sets the shop posted payment method.
		/// </summary>
		public string? ShopPostedPaymentMethod { get; set; }

		/// <summary>
		/// Gets or sets the signature.
		/// </summary>
		public string? Signature { get; set; }

		protected override void PopulateDomainResponse(UpdateOrderCommand domainResponse)
		{
			domainResponse.OrderId = ShoppingCartId;
		}

		public OrderConfirmationModel PopulateOrderConfirmationModel()
		{
            OrderConfirmationModel orderConfirmationModel = new OrderConfirmationModel();

            orderConfirmationModel.CustomerFirstName = CustomerFirstName;
			orderConfirmationModel.CustomerLastName = CustomerSurname;
			orderConfirmationModel.CustomerAddress = CustomerAddress;
			orderConfirmationModel.CustomerCity = CustomerCity;
			orderConfirmationModel.CustomerZIP = CustomerZIP;
			orderConfirmationModel.CustomerCountry = CustomerCountry;
			orderConfirmationModel.CustomerPhone = CustomerPhone;
			orderConfirmationModel.CustomerEmail = CustomerEmail;
			orderConfirmationModel.ShoppingCartId = ShoppingCartId;
			orderConfirmationModel.Amount = Amount!.Contains(',') ? Amount.Replace(',', '.') : $"{Amount}.00";
			orderConfirmationModel.Lang = Lang;

			return orderConfirmationModel;

        }
	}
}
