using BigCommerceApi.Domain.Projections.StoreConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Projections.Auth
{
    public class LoginQuery : IQuery<Response<LoginProjection>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
