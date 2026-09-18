using System;

namespace BigCommerceApi.Domain.Services.WSPayForm.Interaction
{
	public sealed class CreateFormTransactionResponse : FormAPIResponse<CreateFormTransactionResponse.ServiceResponse>
	{
		public CreateFormTransactionResponse(ServiceResponse? serviceResponse = null, Exception? raisedException = null) : base(serviceResponse, raisedException)
		{

		}
		public sealed class ServiceResponse
		{
			public string? TransactionId { get; set; }
			public string? PaymentFormUrl { get; set; }
			public bool HasInformation { get; set; }
			public bool HasWarnings { get; set; }
			public bool HasErrors { get; set; }
			public bool HasSuccesses { get; set; }
		}
	}
}
