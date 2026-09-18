using BigCommerceApi.Domain.Services.Shops.UpdateShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Services.Shops.DeleteShop
{
    public class DeleteShopCommand : ICommand<Response<DeleteShopResult>>
    {
        public string? Id { get; set; }
    }
}
