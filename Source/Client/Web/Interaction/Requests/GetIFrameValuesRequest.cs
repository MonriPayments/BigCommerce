using BigCommerceApi.Domain.Services.Checkout.GetIFrameValues;

namespace BigCommerceApi.Client.Web.Interaction.Requests
{
    public class GetIFrameValuesRequest : BaseAPIRequest<GetIFrameValuesCommand>
    {
        public string? CheckoutID { get; set; }
        public string? SiteUrl { get; set; }
        protected override void PopulateDomainRequest(GetIFrameValuesCommand domainRequest)
        {
            domainRequest.SiteUrl = SiteUrl;
            domainRequest.CheckoutID = CheckoutID;
        }
    }
}
