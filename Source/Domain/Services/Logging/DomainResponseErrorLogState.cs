using WebStudio.Logging.Abstractions;

namespace BigCommerceApi.Domain.Services.Logging
{
    public class DomainResponseErrorLogState : LogState
    {
        public DomainResponseErrorLogState(string commandName, string requestId, object response)
        {
            RequestId = requestId;
            CommandName = commandName;
            Response = response;
        }

        public string RequestId { get; set; }
        public string CommandName { get; set; }
        public object Response { get; set; }
    }
}
