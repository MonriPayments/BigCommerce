namespace BigCommerceApi.Client.Web.Interaction.Responses
{
	public abstract class BaseAPIResponse<TDomainResponse> where TDomainResponse : new()
	{
		private readonly string _requestId = Guid.NewGuid().ToString();

		protected HttpContext? HttpContext;

		public string RequestId => _requestId;

		public void SetHttpContext(HttpContext httpContext) => HttpContext = httpContext;

		public TDomainResponse CreateDomainResponse()
		{
			var domainResponse = new TDomainResponse();
			PopulateDomainResponse(domainResponse);
			return domainResponse;
		}

		protected abstract void PopulateDomainResponse(TDomainResponse domainResponse);
	}
}
