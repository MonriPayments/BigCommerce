using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BigCommerceApi.Client.Web.Pages
{
    public class OrderConfirmationModel : PageModel
    {
        public string? CustomerFirstName { get; set; }
        public string? CustomerLastName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerCity { get; set; }
        public string? CustomerZIP { get; set; }
        public string? CustomerCountry { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }
        public string? ShoppingCartId { get; set; }
        public string? Amount { get; set; }
        public string? MerchantSiteUrl { get; set; }
        public string? CurrencyCode { get; set; }
        public string? Lang { get; set; }
        public string? MerchantName { get; set; }
        public string? MerchantEmail { get; set; }
        public bool ShowMerchantInfo { get; set; }

        public void OnGet(
            string customerFirstName, 
            string customerLastName, 
            string customerAddress, 
            string customerCity, 
            string customerZIP, 
            string customerCountry, 
            string customerPhone, 
            string customerEmail, 
            string shoppingCartId,
            string amount,
            string merchantSiteUrl,
            string currencyCode,
            string lang,
            string merchantName,
            string merchantEmail
            )
        {
            CustomerFirstName = customerFirstName;
            CustomerLastName = customerLastName;
            CustomerAddress = customerAddress;
            CustomerCity = customerCity;
            CustomerZIP = customerZIP;
            CustomerCountry = customerCountry;
            CustomerPhone = customerPhone;
            CustomerEmail = customerEmail;
            ShoppingCartId = shoppingCartId;
            Amount = amount;
            MerchantSiteUrl = merchantSiteUrl;
            CurrencyCode = currencyCode;
            Lang = lang;
            MerchantName = merchantName;
            MerchantEmail = merchantEmail;
            ShowMerchantInfo = !string.IsNullOrEmpty(merchantName) && !string.IsNullOrEmpty(merchantEmail);
        }
    }
}
