using System;
using WebStudio.Logging.Abstractions;

namespace BigCommerceApi.Domain.Services.Logging
{
	public sealed class APIResponseLogState : APICommunicationLogState
	{
		public APIResponseLogState(string apiName, string? requestId, Exception? exception) : this(apiName, requestId, string.Empty, null, exception)
		{
		}

		public APIResponseLogState(string apiName, string? requestId, object? responseBody, int? statusCode, Exception? exception = null) : base(apiName)
		{
			RequestId = requestId;
			ResponseBody = responseBody;
			StatusCode = statusCode;
			Exception = exception == null ? null : new ExceptionLogState(exception);
		}

		public string? RequestId { get; }
		public object? ResponseBody { get; }
		public int? StatusCode { get; }
		public ExceptionLogState? Exception { get; }
	}
}
