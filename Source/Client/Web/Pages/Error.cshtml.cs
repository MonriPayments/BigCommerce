using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BigCommerceApi.Client.Web.Pages
{
    public class ErrorModel : PageModel
    {
        public string? SiteUrl { get; set; }
        public string? Lang { get; set; }
        public void OnGet(string siteUrl, string lang)
        {
            SiteUrl = siteUrl;
            Lang = lang;
        }
    }
}
