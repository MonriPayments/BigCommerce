using BigCommerceApi.Domain.Model.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigCommerceApi.Domain.Projections.Shops
{
    public class GetShopProjection
    {
        public Guid Id { get; set; }
        public string? ShopID { get; set; }
        public string? SecretKey { get; set; }
        public string? Language { get; set; }
        public bool? IsTokenShopId { get; set; }
    }
}

