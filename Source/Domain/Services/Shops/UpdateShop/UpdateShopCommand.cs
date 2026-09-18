using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Services.Shops.UpdateShop
{
    public class UpdateShopCommand : ICommand<Response<UpdateShopResult>>
    {
        public string? Id { get; set; }
        public string? ShopID { get; set; }
        public string? SecretKey { get; set; }
        public string? Language { get; set; }
        public bool? IsTokenShopId { get; set; }
    }
}
