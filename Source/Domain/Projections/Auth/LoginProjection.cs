using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BigCommerceApi.Domain.Projections.Auth
{
    public class LoginProjection
    {
        public string Token { get; set; }
    }
}
