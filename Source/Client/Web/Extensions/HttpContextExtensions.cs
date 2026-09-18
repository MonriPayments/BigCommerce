namespace BigCommerceApi.Client.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetEndpointPath(this HttpContext context) => context.Request.Path.Value!;
        public static HttpMethod GetHttpMethod(this HttpContext context) => new HttpMethod(context.Request.Method);
        public static Dictionary<string, string> GetRequestHeaders(this HttpContext context) => context.Request.Headers.ToDictionary(_ => _.Key, _ => string.Join(";", _.Value));
    }
}
