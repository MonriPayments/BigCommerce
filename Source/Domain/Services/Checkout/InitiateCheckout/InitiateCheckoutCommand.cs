using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Services.Checkout.InitiateCheckout
{
    public class InitiateCheckoutCommand : ICommand<Response<InitiateCheckoutResult>>
    {
        public string? CheckoutID { get; set; }
        public string? SiteUrl { get; set; }
        public string? RequestId { get; set; }
    }
}
