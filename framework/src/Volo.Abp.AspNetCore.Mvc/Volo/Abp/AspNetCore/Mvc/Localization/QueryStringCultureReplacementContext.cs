using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace Volo.Abp.AspNetCore.Mvc.Localization
{
    public class QueryStringCultureReplacementContext
    {
        public HttpContext HttpContext { get; }

        public RequestCulture RequestCulture { get; }

        public string ReturnUrl { get; set; }

        public QueryStringCultureReplacementContext(HttpContext httpContext, RequestCulture requestCulture, string returnUrl)
        {
            HttpContext = httpContext;
            RequestCulture = requestCulture;
            ReturnUrl = returnUrl;
        }
    }
}
