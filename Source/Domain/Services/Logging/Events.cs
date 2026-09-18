using System.Linq;
using System.Reflection;

namespace BigCommerceApi.Domain.Services.Logging
{
	public static class Events
	{
		private static readonly string[] AllEvents;

		static Events() => AllEvents = typeof(Events)
				  .GetFields(BindingFlags.Public | BindingFlags.Static)
				  .Where(fieldInfo => fieldInfo.IsStatic && fieldInfo.FieldType == typeof(string))
				  .Select(fieldInfo => fieldInfo.GetValue(null))
				  .Cast<string>()
				  .ToArray();

		public static readonly string FatalErrorInApplication = nameof(FatalErrorInApplication);

		public static readonly string WSPayAPIRequestSent = nameof(WSPayAPIRequestSent);
		public static readonly string WSPayAPIResponseReceived = nameof(WSPayAPIResponseReceived);
		public static readonly string BigCCreateOrderFromCartFailed = nameof(BigCCreateOrderFromCartFailed);
		public static readonly string WSPayAPIResponseError = nameof(WSPayAPIResponseError);
		public static readonly string WSPayStatusCheckError = nameof(WSPayStatusCheckError);
        public static readonly string InitiateCheckoutRequestReceived = nameof(InitiateCheckoutRequestReceived);
        public static readonly string InitiateCheckoutResponseSent = nameof(InitiateCheckoutResponseSent);
        public static readonly string BigCUpdateOrderRequestSent = nameof(BigCUpdateOrderRequestSent);
        public static readonly string BigCUpdateOrderRequestReceived = nameof(BigCUpdateOrderRequestReceived);
        public static readonly string BigCCreateWebhookRequestSent = nameof(BigCCreateWebhookRequestSent);
        public static readonly string BigCCreateWebhookRequestReceived = nameof(BigCCreateWebhookRequestReceived);
        public static readonly string CallbackRequestReceived = nameof(CallbackRequestReceived);
        public static readonly string CallbackResponseSent = nameof(CallbackResponseSent);
        public static readonly string DomainResponseErrorOccured = nameof(DomainResponseErrorOccured);
        public static readonly string BigCAuthResponseSent = nameof(BigCAuthResponseSent);
        public static readonly string BigCAuthRequestReceived = nameof(BigCAuthRequestReceived);
        public static readonly string GetCustomerTokensRequestReceived = nameof(GetCustomerTokensRequestReceived);
        public static readonly string GetTransactionRequestReceived = nameof(GetTransactionRequestReceived);
        public static readonly string GetTransactionResponseReceived = nameof(GetTransactionResponseReceived);
        public static readonly string BigCLoadRequestReceived = nameof(BigCLoadRequestReceived);
        public static readonly string BigCLoadResponseReceived = nameof(BigCLoadResponseReceived);
        public static readonly string BigCgetCustomerTokensRequestReceived = nameof(BigCgetCustomerTokensRequestReceived);
        public static readonly string BigCScriptCommunication = nameof(BigCScriptCommunication);
        public static readonly string BigCGetMerchantProp = nameof(BigCGetMerchantProp);
        public static readonly string BigCMerchantSiteRequest = nameof(BigCMerchantSiteRequest);
        public static readonly string BigCCheckoutCommunication = nameof(BigCCheckoutCommunication);
        public static readonly string BigCGetCheckoutOrderResponse = nameof(BigCGetCheckoutOrderResponse);
        public static readonly string BigCGetCheckoutOrderError = nameof(BigCGetCheckoutOrderError);
        public static readonly string BigCGetCheckoutResponse = nameof(BigCGetCheckoutResponse);
        public static readonly string UpdateOrderHandler = nameof(UpdateOrderHandler);
        public static readonly string UpdateOrderOnCallback = nameof(UpdateOrderOnCallback);
        public static readonly string UpdateOrderResponse = nameof(UpdateOrderResponse);
        public static readonly string GetIframeValuesRequest = nameof(GetIframeValuesRequest);
        public static readonly string GetIframeValuesResponse = nameof(GetIframeValuesResponse);

        internal static bool EventIsDefined(string eventId)
		{
			return AllEvents.Any(_ => _ == eventId);
		}
	}

}
