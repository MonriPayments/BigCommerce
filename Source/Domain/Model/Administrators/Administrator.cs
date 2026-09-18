using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStudio.Entities.Core;

namespace BigCommerceApi.Domain.Model.Administrators
{
    public sealed class Administrator : Entity
    {
        public string? Password { get; set; }
        public string? Email { get; set; }
        public DateTime? LastLogin { get; set; } = DateTime.UtcNow;
    }
}
