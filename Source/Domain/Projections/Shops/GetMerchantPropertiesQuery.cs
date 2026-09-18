using System.Collections.Generic;
using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Projections.Shops
{
    public class GetMerchantPropertiesQuery : IQuery<List<MerchantPropertiesProjection>>
    {
        public string SubjectPath { get; set; }
    }
}
