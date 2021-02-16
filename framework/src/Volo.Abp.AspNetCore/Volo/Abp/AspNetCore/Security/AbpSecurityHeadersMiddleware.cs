using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Volo.Abp.AspNetCore.Security
{
    public class AbpSecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public AbpSecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            /*X-Content-Type-Options header tells the browser to not try and “guess” what a mimetype of a resource might be, and to just take what mimetype the server has returned as fact.*/
            AddHeaderIfNotExists(httpContext, "X-Content-Type-Options", "nosniff");

            /*X-XSS-Protection is a feature of Internet Explorer, Chrome and Safari that stops pages from loading when they detect reflected cross-site scripting (XSS) attacks*/
            AddHeaderIfNotExists(httpContext, "X-XSS-Protection", "1; mode=block");

            /*The X-Frame-Options HTTP response header can be used to indicate whether or not a browser should be allowed to render a page in a <frame>, <iframe> or <object>. SAMEORIGIN makes it being displayed in a frame on the same origin as the page itself. The spec leaves it up to browser vendors to decide whether this option applies to the top level, the parent, or the whole chain*/
            AddHeaderIfNotExists(httpContext, "X-Frame-Options", "SAMEORIGIN");

            await _next.Invoke(httpContext);
        }

        private static void AddHeaderIfNotExists(HttpContext context, string key, string value)
        {
            if (!context.Response.Headers.ContainsKey(key))
            {
                context.Response.Headers.Add(key, value);
            }
        }
    }
}