namespace BigCommerceApi.Client.Web.Interaction.Requests
{
    public abstract class BaseAPIRequest<TDomainRequest> where TDomainRequest : new()
    {
        private readonly string _requestId = Guid.NewGuid().ToString();

        protected HttpContext? HttpContext;

        public string RequestId => _requestId;

        public void SetHttpContext(HttpContext httpContext) => HttpContext = httpContext;

        public TDomainRequest CreateDomainRequest()
        {
            var domainRequest = new TDomainRequest();
            PopulateDomainRequest(domainRequest);
            return domainRequest;
        }

        protected abstract void PopulateDomainRequest(TDomainRequest domainRequest);
    }
}
