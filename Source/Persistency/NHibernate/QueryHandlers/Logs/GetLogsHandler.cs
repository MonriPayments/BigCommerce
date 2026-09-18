using BigCommerceApi.Domain.Model.Logs;
using BigCommerceApi.Domain.Projections.Logs;
using BigCommerceApi.Domain.Projections.StoreConfigurations;
using BigCommerceApi.Domain.Services;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Persistency.NHibernate.QueryHandlers.Logs
{
    public class GetLogsHandler : BaseCommandHandler, IAsyncQueryHandler<GetLogsQuery, Response<GetLogsProjection>>
    {
        private readonly ILogger _logger;
        public GetLogsHandler(IQueryExecutor queryExecutor, UnitOfWork unitOfWork, ILogger logger) : base(queryExecutor, unitOfWork)
        {
            _logger = logger;
        }

        public async Task<Response<GetLogsProjection>> ExecuteAsync(GetLogsQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), "Query cannot be null.");
            }
            try
            {
                // handle the case where FromDate is after ToDate
                if (query.FromDate > query.ToDate)
                {
                    return ResponseHelper.CreateErrorResponse<GetLogsProjection>("FromDate cannot be after ToDate.");
                }
                // handle if they are the same date --> get all logs from that date
                if (query.FromDate.Date == query.ToDate.Date)
                {
                    query.ToDate = query.ToDate.AddDays(1).AddTicks(-1); // set ToDate to the end of the day
                }
                var logs = await QueryExecutor.GetAllAsync<Log>(
                    log => log.CreatedOn >= query.FromDate && log.CreatedOn <= query.ToDate,
                    cancellationToken
                );
                var logList = logs.Select(log => new LogDto
                {
                    Id = log.Id,
                    CreatedOn = log.CreatedOn,
                    Level = log.Level,
                    EventId = log.EventId,
                    State = log.State
                }).ToList();

                var result = new GetLogsProjection { Logs = logList };
                return new Response<GetLogsProjection>(result);
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "GetLogsHandler" },
                };
                _logger.LogApplicationError(ex, additionalData);
                return ResponseHelper.CreateErrorResponse<GetLogsProjection>(ErrorMessages.GenericError);
            }
        }

    }
}
