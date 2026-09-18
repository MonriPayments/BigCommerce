using BigCommerceApi.Domain.Model.Administrators;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigCommerceApi.Persistency.NHibernate.Mappings
{
    public class AdministratorMap : ClassMap<Administrator>
    {
        public AdministratorMap() {
            Id(_ => _.Id);

            Map(_ => _.Password);
            Map(_ => _.Email);
            Map(_ => _.LastLogin);
        }
    }
}

