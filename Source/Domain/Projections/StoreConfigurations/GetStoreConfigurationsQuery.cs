using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStudio.Entities.Interaction;
using BigCommerceApi.Domain.Projections.StoreConfigurations;

namespace BigCommerceApi.Domain.Projections.StoreConfigurations
{
    public class GetStoreConfigurationsQuery : IQuery<Response<GetStoreConfigurationsProjection>>
    {
    }
}
