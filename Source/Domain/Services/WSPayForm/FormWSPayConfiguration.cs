using System;

namespace BigCommerceApi.Domain.Services.WSPayForm
{
	public class FormWSPayConfiguration
	{
		public string? BaseUrlTest { get; set; }
		public string? BaseUrlProd { get; set; }
		public string? IframeAuthorizationUrlTest { get; set; }
		public string? IframeAuthorizationUrlProd { get; set; }
		public string? CreateTransactionUrlEndpoint { get; set; }
		public string? MediaType { get; set; }
		public string? CreateFormTransactionRequestCacheKey { get; set; }
		public string? FormRedirectRequestRequestCacheKey { get; set; }
		public int DefaultTimeoutInSeconds { get; set; }
		public TimeSpan DefaultTimeout => TimeSpan.FromSeconds(DefaultTimeoutInSeconds);
	}
}
