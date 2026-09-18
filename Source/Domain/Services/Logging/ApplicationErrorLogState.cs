using System;
using System.Collections.Generic;
using WebStudio.Logging.Abstractions;

namespace BigCommerceApi.Domain.Services.Logging
{
	internal class ApplicationErrorLogState : LogState
	{
		public ApplicationErrorLogState(Exception exception, IDictionary<string, string>? additionalData = null)
		{
			Exception = new ExceptionLogState(exception);
			AdditionalData = additionalData;
		}

		public ExceptionLogState Exception { get; }
		public IDictionary<string, string>? AdditionalData { get; }
		public override string? LogType => Logging.LogType.ApplicationError.ToString();
	}
}
