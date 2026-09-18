using WebStudio.Common.Contracts;
using WebStudio.Logging.Abstractions;

namespace BigCommerceApi.Domain.Services.Logging
{
	public abstract class APICommunicationLogState : LogState
	{
		protected APICommunicationLogState(string apiName)
		{
			Argument.IsNotNullOrWhiteSpace(apiName, nameof(apiName));

			APIName = apiName;
		}

		public string APIName { get; }

		public override string? LogType => Logging.LogType.APICommunication.ToString();
	}
}
