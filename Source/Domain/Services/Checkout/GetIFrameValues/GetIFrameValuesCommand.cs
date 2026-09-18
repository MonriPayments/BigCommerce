using System.Collections.Generic;
using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Services.Checkout.GetIFrameValues
{
    public class GetIFrameValuesCommand : ICommand<Response<List<GetIFrameValuesResult>>>
    {
        public string? CheckoutID { get; set; }
        public string? SiteUrl { get; set; }
        public string? RequestId { get; set; }
    }
}
