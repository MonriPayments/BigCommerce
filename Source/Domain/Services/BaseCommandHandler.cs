using WebStudio.Common.Contracts;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Services
{
    public abstract class BaseCommandHandler
    {
        protected BaseCommandHandler(IQueryExecutor queryExecutor, UnitOfWork unitOfWork)
        {
            Argument.IsNotNull(queryExecutor, nameof(queryExecutor));
            Argument.IsNotNull(unitOfWork, nameof(unitOfWork));

            QueryExecutor = queryExecutor;
            UnitOfWork = unitOfWork;
        } 

        protected IQueryExecutor QueryExecutor { get; }
        protected UnitOfWork UnitOfWork { get; }
    }
}
