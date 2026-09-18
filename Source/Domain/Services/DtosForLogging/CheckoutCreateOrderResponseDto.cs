using BigCommerceApi.Domain.Services.RestManagementApi.Interaction;

namespace BigCommerceApi.Domain.Services.DtosForLogging
{
    public class CheckoutCreateOrderResponseDto
    {
        public int? Id { get; set; }
        public object? Meta { get; set; }
        public string? Error { get; set; }
    }

    public class GetCheckoutResponseDto
    {
        public string? ShoppingCartId { get; set; }
        public double? TotalAmount { get; set; }
        public string? CustomerFirstName { get; set; }
        public string? CustomerLastName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerCity { get; set; }
        public string? CustomerZIP { get; set; }
        public string? CustomerCountry { get; set; }
        public string? CustomerAddress { get; set; }
    }

    public class UpdateResponseDto
    {
        public string? Error { get; set;}
    }

    public static class PrepareForLog
    {
        public static CheckoutCreateOrderResponseDto PrepareCheckoutCreateOrderResponseForLogging(CheckoutCreateOrderResponse.ServiceResponse serviceResponse)
        {
            return new CheckoutCreateOrderResponseDto
            {
                Id = serviceResponse.Data?.Id,
                Meta = serviceResponse?.Meta,
                Error = serviceResponse?.Error
            };
        }

        public static GetCheckoutResponseDto? PrepareGetCheckoutResponseForLogging(GetCheckoutResponse.ServiceResponse serviceResponse)
        {
            if (serviceResponse.Data == null)
                return new GetCheckoutResponseDto();

            if (serviceResponse.Data.BillingAddress == null)
                return new GetCheckoutResponseDto();

            return new GetCheckoutResponseDto
            {
                ShoppingCartId = serviceResponse.Data?.OrderId,
                TotalAmount = serviceResponse.Data?.GrandTotal,
                CustomerFirstName = serviceResponse.Data?.BillingAddress?.FirstName,
                CustomerLastName = serviceResponse.Data?.BillingAddress?.LastName,
                CustomerEmail = serviceResponse.Data?.BillingAddress?.Email,
                CustomerPhone = serviceResponse.Data?.BillingAddress?.Phone,
                CustomerCity = serviceResponse.Data?.BillingAddress?.City,
                CustomerZIP = serviceResponse.Data?.BillingAddress?.PostalCode,
                CustomerCountry = serviceResponse.Data?.BillingAddress?.CountryCode,
                CustomerAddress = serviceResponse.Data?.BillingAddress?.Address1
            };
        }

        public static UpdateResponseDto PrepareUpdateOrderResponseForLogging(UpdateOrderResponse.ServiceResponse updateOrderResponse)
        {
            return new UpdateResponseDto
            {
                Error = updateOrderResponse.Error
            };
        }
    }
}
