using BigCommerceApi.Domain.Projections.Transactions;
using BigCommerceApi.Domain.Services.Logging;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Entities.Interaction;
using WebStudio.Persistency.NHibernateCore.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Persistency.NHibernate.QueryHandlers.Transactions
{
    public sealed class GetTransactionsHandler : BaseQueryHandler, IAsyncQueryHandler<GetTransactionsQuery, IList<TransactionProjection>>
    {
        private readonly ILogger _logger;
        public GetTransactionsHandler(ISession session, ILogger logger) : base(session) 
        {
            _logger = logger;
        }

        public async Task<IList<TransactionProjection>> ExecuteAsync(GetTransactionsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var transactionDetails = Session.CreateSQLQuery("EXEC Transactions_GetTransactionsByDates :DateFrom, :DateTo, :PageSize, :PageNumber, :ShopID, :CustomerFirstName, :CustomerLastName, :ShoppingCartID, :CreditCardName")
                    .AddEntity(typeof(TransactionProjection))
                    .SetParameter("DateFrom", query.CreatedFrom)
                    .SetParameter("DateTo", query.CreatedUntil)
                    .SetParameter("PageSize", query.PageSize)
                    .SetParameter("PageNumber", query.PageNumber)
                    .SetParameter("ShopID", query.ShopID)
                    .SetParameter("CustomerFirstName", query.CustomerFirstName)
                    .SetParameter("CustomerLastName", query.CustomerLastName)
                    .SetParameter("ShoppingCartID", query.ShoppingCartID)
                    .SetParameter("CreditCardName", query.CreditCardName);
            
                var result = await transactionDetails.ListAsync<TransactionProjection>();

                return result;
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "GetTransactionsHandler" },
                };
                _logger.LogApplicationError(ex, additionalData);
                return null;
            }
        }
    }
}
