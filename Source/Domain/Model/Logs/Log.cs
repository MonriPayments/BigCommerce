using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStudio.Entities.Core;

namespace BigCommerceApi.Domain.Model.Logs
{
    public sealed class Log : Entity
    {
        public DateTimeOffset? CreatedOn { get; set; }
        public string? Level { get; set; } 
        public string? EventId { get; set; } 
        public string? State { get; set; }
    }
}
