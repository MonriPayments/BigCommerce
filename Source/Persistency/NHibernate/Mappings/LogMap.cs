using BigCommerceApi.Domain.Model.Administrators;
using BigCommerceApi.Domain.Model.Logs;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigCommerceApi.Persistency.NHibernate.Mappings
{
    public class LogMap : ClassMap<Log>
    {
        public LogMap()
        {
            Id(_ => _.Id);
            Map(_ => _.CreatedOn).Nullable();
            Map(_ => _.Level).Nullable();
            Map(_ => _.EventId).Nullable();
            Map(_ => _.State).Nullable();
        }
    }
}
