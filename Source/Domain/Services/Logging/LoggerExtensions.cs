using System;
using System.Collections.Generic;
using WebStudio.Common.Contracts;
using WebStudio.Logging.Abstractions;

namespace BigCommerceApi.Domain.Services.Logging
{
	public static class LoggerExtensions
	{
		private const string EventIdIsNotDefined = "Event id is not defined.";

		public static void LogApplicationError(this ILogger logger, Exception exception, IDictionary<string, string>? additionalData)
		{
			Argument.IsNotNull(logger, nameof(logger));

			var logState = new ApplicationErrorLogState(exception, additionalData);
			logger.Log(new Log(LogLevel.Fatal, Events.FatalErrorInApplication, logState));
		}

		public static void LogAPICommunication(this ILogger logger, string eventId, APICommunicationLogState logState, LogLevel logLevel = LogLevel.Information)
		{
			Argument.IsNotNull(logger, nameof(logger));
			Argument.IsValid(Events.EventIsDefined(eventId), EventIdIsNotDefined, nameof(eventId));
			Argument.IsNotNull(logState, nameof(logState));

			logger.Log(new Log(logLevel, eventId, logState));
		}

        public static void LogDomainResponseError(this ILogger logger, DomainResponseErrorLogState logState)
        {
            logger.Log(new Log(LogLevel.Error, Events.DomainResponseErrorOccured, logState));
        }
    }
}
