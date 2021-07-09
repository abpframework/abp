using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace Volo.Abp.AspNetCore.Mvc.Localization
{
    public class QueryStringCultureReplacementContext
    {
        public HttpContext HttpContext { get; set; }

        public string ReturnUrl { get; set; }

        public RequestCulture RequestCulture { get; set; }
    }
}
