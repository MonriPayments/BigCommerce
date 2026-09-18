using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Services.StoreConfigurations.AddShopToStoreConfiguration
{
    public class AddShopToStoreConfigurationCommand : ICommand<Response<AddShopToStoreConfigurationResult>>
    {
        public string? ShopID { get; set; }
        public string? SecretKey { get; set; }
        public string? Language { get; set; }
        public bool? IsTokenShopId { get; set; }
        public string? StoreConfigurationId { get; set; }
    }
}
