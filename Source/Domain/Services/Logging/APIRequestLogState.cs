using System.Collections.Generic;
using System.Net.Http;

namespace BigCommerceApi.Domain.Services.Logging
{
	public sealed class APIRequestLogState : APICommunicationLogState
	{
		public APIRequestLogState(string apiName, string? requestId, IDictionary<string, string>? requestHeaders, HttpMethod method, string endpoint) : this(apiName, requestId, string.Empty, requestHeaders, method, endpoint)
		{
		}

		public APIRequestLogState(string apiName, string? requestId, object? requestBody, IDictionary<string, string>? requestHeaders, HttpMethod method, string? endpoint) : base(apiName)
		{
			RequestId = requestId;
			RequestBody = requestBody;
			RequestHeaders = requestHeaders;
			HttpMethod = method.ToString();
			Endpoint = endpoint;
		}

        public APIRequestLogState(string apiName, string? requestId, object? requestBody) : base(apiName)
        {
            RequestId = requestId;
            RequestBody = requestBody;
        }

        public string? RequestId { get; }
		public object? RequestBody { get; }
		public IDictionary<string, string>? RequestHeaders { get; }
		public string? HttpMethod { get; }
		public string? Endpoint { get; }
	}

}
