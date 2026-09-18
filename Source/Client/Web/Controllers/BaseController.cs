using Microsoft.AspNetCore.Mvc;
using WebStudio.Common.Contracts;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Client.Web.Controllers
{
    public class BaseController : Controller
	{
        protected readonly IQueryExecutor QueryExecutor;
        protected readonly ICommandExecutor CommandExecutor;
        protected readonly ILogger Logger;

		protected BaseController(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, ILogger logger)
		{
			Argument.IsNotNull(queryExecutor, nameof(queryExecutor));
			Argument.IsNotNull(commandExecutor, nameof(commandExecutor));
			Argument.IsNotNull(logger, nameof(logger));

			QueryExecutor = queryExecutor;
			CommandExecutor = commandExecutor;
			Logger = logger;
		}
	}
}
