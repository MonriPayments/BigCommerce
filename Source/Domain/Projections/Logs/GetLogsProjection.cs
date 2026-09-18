using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigCommerceApi.Domain.Projections.Logs
{
    public class GetLogsProjection
    {
        public List<LogDto> Logs { get; set; } = new List<LogDto>();
    }
    public class LogDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string? Level { get; set; }
        public string? EventId { get; set; }
        public string? State { get; set; }
    }
}
