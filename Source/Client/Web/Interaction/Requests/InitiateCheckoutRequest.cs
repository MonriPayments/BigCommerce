using BigCommerceApi.Domain.Services.Checkout.InitiateCheckout;

namespace BigCommerceApi.Client.Web.Interaction.Requests
{
    public class InitiateCheckoutRequest : BaseAPIRequest<InitiateCheckoutCommand>
    {
        public string? CheckoutID { get; set; }
        public string? SiteUrl { get; set; }
        protected override void PopulateDomainRequest(InitiateCheckoutCommand domainRequest)
        {
            domainRequest.SiteUrl = SiteUrl;
            domainRequest.CheckoutID = CheckoutID;
        }
    }
}
