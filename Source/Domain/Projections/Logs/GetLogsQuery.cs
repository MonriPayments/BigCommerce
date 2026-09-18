using BigCommerceApi.Domain.Projections.StoreConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Projections.Logs
{
    public class GetLogsQuery : IQuery<Response<GetLogsProjection>>
    {
        public GetLogsQuery(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
    }
}
