using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Projections.Shops
{
    public class GetShopsQuery : IQuery<List<GetShopProjection>>
    {
        public GetShopsQuery(string storeConfigurationId)
        {
            StoreConfigurationId = storeConfigurationId;
        }
        public string StoreConfigurationId { get; set; }
    }
}
