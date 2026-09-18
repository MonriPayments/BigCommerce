using System;

namespace BigCommerceApi.Domain.Services.RestManagementApi.Interaction
{
	public class CheckoutCreateOrderResponse : RestManagementApiResponse<CheckoutCreateOrderResponse.ServiceResponse>
	{
		public CheckoutCreateOrderResponse(ServiceResponse? serviceResponse = null, Exception? raisedException = null) : base(serviceResponse, raisedException)
		{

		}

		public class ServiceResponse
		{
			public Data? Data { get; set; }
			public object? Meta { get; set; }
			public string? Error { get; set; }
		}

		public class Data
		{
			public int? Id { get; set; }
		}
    }
}
