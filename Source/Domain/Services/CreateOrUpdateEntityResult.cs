using System;

namespace BigCommerceApi.Domain.Services
{
    public class CreateOrUpdateEntityResult
    {
        public CreateOrUpdateEntityResult(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
