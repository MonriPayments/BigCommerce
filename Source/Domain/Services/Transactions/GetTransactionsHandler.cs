using BigCommerceApi.Domain.Services.Cache;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
using BigCommerceApi.Domain.Services.RestManagementApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Common.Contracts;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Domain.Services.Transactions
{
    public class GetTransactionsHandler : BaseCommandHandler, IAsyncCommandHandler<GetTransactionCommand, Response<GetTransactionsResult>>
    {
        private readonly IRestManagementApi _restManagementApi;
        private readonly IWebAppCache _memoryCache;
        private readonly ILogger _logger;

        public GetTransactionsHandler(IQueryExecutor queryExecutor, UnitOfWork unitOfWork, IRestManagementApi restManagementApi, IWebAppCache memoryCache, ILogger logger) : base(queryExecutor, unitOfWork)
        {
            Argument.IsNotNull(restManagementApi, nameof(restManagementApi));
            Argument.IsNotNull(memoryCache, nameof(memoryCache));

            _restManagementApi = restManagementApi;
            _memoryCache = memoryCache;
            _logger = logger;

        }

        public async Task<Response<GetTransactionsResult>> ExecuteAsync(GetTransactionCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var response = new Response();

                var transactions = await QueryExecutor.GetAllAsync<Model.Transactions.Transaction>(cancellationToken);

                if (transactions == null)
                    return new Response<GetTransactionsResult>(new GetTransactionsResult { });

                var transactionlist = TransactionsHelpers.GetTransactionList(transactions.ToList());

                var transactionResult = new GetTransactionsResult { TransactionsResult = transactionlist };

                return new Response<GetTransactionsResult>(transactionResult);
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "GetTransactionsHandler" },
                };
                _logger.LogApplicationError(ex, additionalData);
                return ResponseHelper.CreateErrorResponse<GetTransactionsResult>(ErrorMessages.GenericError);
            }
        }


    }
}
