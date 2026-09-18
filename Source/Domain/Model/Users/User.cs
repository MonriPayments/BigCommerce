using BigCommerceApi.Domain.Model.Stores;
using System;
using WebStudio.Entities.Core;

namespace BigCommerceApi.Domain.Model.Users
{
    public class User : Entity
    {
        public string? Email { get; set; }
        public Guid? IdStoreConfiguration { get; set; }
        public StoreConfiguration? StoreConfiguration { get; set; }
    }
}
