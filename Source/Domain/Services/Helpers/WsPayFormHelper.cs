using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Services.Checkout.GetIFrameValues;
using BigCommerceApi.Domain.Services.RestManagementApi.Interaction;
using BigCommerceApi.Domain.Services.WSPayForm.Interaction;

namespace BigCommerceApi.Domain.Services.Helpers
{
    public static class WsPayFormHelper
	{
		public static CreateFormTransactionRequest CreateFormTransactionRequest(GetCheckoutResponse.ServiceResponse getCheckoutResponse, BigCommerceApi.Domain.Model.Shops.Shops shop, string orderId, string appUri)
		{
			var amount = getCheckoutResponse?.Data?.GrandTotal!.Value.ToString("0.00").Replace(".", ",");
			var amountInPgFormat = amount?.Replace(",", "").Replace(".", "");
			var version = "2.0";
			var paymentPlan = "0000";
			var checkoutResponseData = getCheckoutResponse?.Data;

			return new CreateFormTransactionRequest
			{
				ShopID = shop?.ShopID,
				ShoppingCartID = orderId,
				Version = version,
				TotalAmount = amount,
				CustomerFirstName = checkoutResponseData?.BillingAddress?.FirstName,
				CustomerLastName = checkoutResponseData?.BillingAddress?.LastName,
				CustomerAddress = checkoutResponseData?.BillingAddress?.Address1,
				CustomerCity = checkoutResponseData?.BillingAddress?.City,
				CustomerZIP = checkoutResponseData?.BillingAddress?.PostalCode,
				CustomerCountry = checkoutResponseData?.BillingAddress?.CountryCode,
				CustomerEmail = checkoutResponseData?.BillingAddress?.Email,
				CustomerPhone = checkoutResponseData?.BillingAddress?.Phone,
				Lang = shop?.Language,
				PaymentPlan = paymentPlan,
				CancelURL = $"{appUri}error",
				ReturnURL = $"{appUri}redirect/order-confirmation",
				ReturnErrorURL = $"{appUri}error",
				Signature = WSPaySignatureGenerator.GenerateForProcessPayment(shop?.ShopID, shop?.SecretKey, orderId, amountInPgFormat),
				CurrencyCode = CurrencyCodeHelper.getNumericCode(checkoutResponseData?.Cart?.Currency?.Code!)
			};
		}

		public static GetIFrameValuesResult CreateIframeRequest(
			GetCheckoutResponse.ServiceResponse getCheckoutResponse,
            BigCommerceApi.Domain.Model.Shops.Shops shop,
			string orderId,
			string appUri,
			StoreConfiguration storeConfiguration,
			string iframeAuthorizationUrl,
			string iframeResponseTarget)
		{
            var amount = getCheckoutResponse?.Data?.GrandTotal.Value.ToString("0.00").Replace(".", ",");
            var amountInPgFormat = amount?.Replace(",", "").Replace(".", "");
            var version = "2.0";
            var paymentPlan = "0000";
            var checkoutResponseData = getCheckoutResponse?.Data;

            return new GetIFrameValuesResult
            {
				Url = iframeAuthorizationUrl,
                ShopID = shop?.ShopID,
                ShoppingCartID = orderId,
                Version = version,
                TotalAmount = amount,
                CustomerFirstName = checkoutResponseData?.BillingAddress?.FirstName,
                CustomerLastName = checkoutResponseData?.BillingAddress?.LastName,
                CustomerAddress = checkoutResponseData?.BillingAddress?.Address1,
                CustomerCity = checkoutResponseData?.BillingAddress?.City,
                CustomerZIP = checkoutResponseData?.BillingAddress?.PostalCode,
                CustomerCountry = checkoutResponseData?.BillingAddress?.CountryCode,
                CustomerEmail = checkoutResponseData?.BillingAddress?.Email,
                CustomerPhone = checkoutResponseData?.BillingAddress?.Phone,
                Lang = shop?.Language,
                PaymentPlan = paymentPlan,
                CancelURL = $"{appUri}error/errorpage",
				ReturnURL = $"{appUri}redirect/order-confirmation",
				ReturnErrorURL = $"{appUri}error/errorpage",
                Signature = WSPaySignatureGenerator.GenerateForProcessPayment(shop?.ShopID, shop?.SecretKey, orderId, amountInPgFormat),
                IFrame = "true" ,
                IFrameResponseTarget = iframeResponseTarget,
                IsTokenRequest = "0",
                CurrencyCode = CurrencyCodeHelper.getNumericCode(checkoutResponseData?.Cart?.Currency?.Code!),
                IsTokenShopID = shop?.IsTokenShopId
            };
        }
    }
}
