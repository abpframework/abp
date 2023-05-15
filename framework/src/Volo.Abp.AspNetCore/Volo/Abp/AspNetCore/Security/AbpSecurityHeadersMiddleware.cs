using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Security;

public class AbpSecurityHeadersMiddleware : IMiddleware, ITransientDependency
{
    public IOptions<AbpSecurityHeadersOptions> Options { get; set; }

    public AbpSecurityHeadersMiddleware(IOptions<AbpSecurityHeadersOptions> options)
    {
        Options = options;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        /*X-Content-Type-Options header tells the browser to not try and “guess” what a mimetype of a resource might be, and to just take what mimetype the server has returned as fact.*/
        AddHeader(context, "X-Content-Type-Options", "nosniff");

        /*X-XSS-Protection is a feature of Internet Explorer, Chrome and Safari that stops pages from loading when they detect reflected cross-site scripting (XSS) attacks*/
        AddHeader(context, "X-XSS-Protection", "1; mode=block");

        /*The X-Frame-Options HTTP response header can be used to indicate whether or not a browser should be allowed to render a page in a <frame>, <iframe> or <object>. SAMEORIGIN makes it being displayed in a frame on the same origin as the page itself. The spec leaves it up to browser vendors to decide whether this option applies to the top level, the parent, or the whole chain*/
        AddHeader(context, "X-Frame-Options", "SAMEORIGIN");

        if (Options.Value.UseContentSecurityPolicyHeader)
        {
            AddHeader(context, "Content-Security-Policy",
                Options.Value.ContentSecurityPolicyValue.IsNullOrEmpty()
                    ? "object-src 'none'; form-action 'self'; frame-ancestors 'none'"
                    : Options.Value.ContentSecurityPolicyValue);
        }
        
        foreach (var (key, value) in Options.Value.Headers)
        {
            AddHeader(context, key, value, true);
        }

        await next.Invoke(context);
    }

    protected virtual void AddHeader(HttpContext context, string key, string value, bool overrideIfExists = false)
    {
        if (overrideIfExists && context.Response.Headers.TryGetValue(key, out _))
        {
            context.Response.Headers[key] = value;
            return;
        }
        
        context.Response.Headers.AddIfNotContains(new KeyValuePair<string, StringValues>(key, value));
    }
}
