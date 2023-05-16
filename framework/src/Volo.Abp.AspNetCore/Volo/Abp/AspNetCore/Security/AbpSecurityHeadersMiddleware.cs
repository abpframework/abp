using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Security;

public class AbpSecurityHeadersMiddleware : IMiddleware, ITransientDependency
{
    public IOptions<AbpSecurityHeadersOptions> Options { get; set; }
    protected const string ScriptSrcKey = "script-src";
    protected const string DefaultValue = "object-src 'none'; form-action 'self'; frame-ancestors 'none'";

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

        var requestAcceptTypeHtml = context.Request.Headers["Accept"].Any(x =>
            x.Contains("text/html") || x.Contains("*/*") || x.Contains("application/xhtml+xml"));

        if (!requestAcceptTypeHtml 
            || !Options.Value.UseContentSecurityPolicyHeader 
            || await AlwaysIgnoreContentTypes(context) 
            || context.GetEndpoint() == null
            || Options.Value.IgnoredNonceScriptPaths.Any(x => context.Request.Path.StartsWithSegments(x.EnsureStartsWith('/'))))
        {
            AddOtherHeaders(context);
            await next.Invoke(context);
            return;
        }

        if (Options.Value.UseContentSecurityPolicyNonce)
        {
            var randomValue = Guid.NewGuid().ToString("N");
            context.Items.Add(AbpAspNetCoreConsts.ScriptNonceKey, randomValue);
        }


        context.Response.OnStarting(() =>
        {
            if (context.Response.Headers.ContainsKey("Content-Security-Policy"))
            {
                return Task.CompletedTask;
            }

            if (context.Response.ContentType?.StartsWith("text/html") != true)
            {
                return Task.CompletedTask;
            }
            
            if (context.Response.StatusCode is < 200 or > 299)
            {
                return Task.CompletedTask;
            }

            AddHeader(context, "Content-Security-Policy", BuildContentSecurityPolicyValue(context));

            return Task.CompletedTask;
        });

        AddOtherHeaders(context);
        await next.Invoke(context);
    }
    
    private async Task<bool> AlwaysIgnoreContentTypes(HttpContext context)
    {
        foreach (var selector in Options.Value.IgnoredNonceScriptSelectors)
        {
            if(await selector(context))
            {
                return true;
            }
        }
        
        return false;
    }

    private void AddOtherHeaders(HttpContext context)
    {
        foreach (var (key, value) in Options.Value.Headers)
        {
            AddHeader(context, key, value, true);
        }
    }

    protected virtual string BuildContentSecurityPolicyValue(HttpContext context)
    {
        if (!(Options.Value.UseContentSecurityPolicyNonce &&
              context.Items.TryGetValue(AbpAspNetCoreConsts.ScriptNonceKey, out var nonce) &&
              nonce is string nonceValue && !string.IsNullOrEmpty(nonceValue)))
        {
            return ContentSecurityPolicyValuesToCSPString();
        }

        var scriptSrcValue = "";
        if (Options.Value.ContentSecurityPolicyValues.TryGetValue(ScriptSrcKey, out var scriptSrc))
        {
            scriptSrcValue = string.Join(" ", scriptSrc);
        }

        scriptSrcValue += $" 'nonce-{nonceValue}'";

        return ContentSecurityPolicyValuesToCSPString(true) + $"; {ScriptSrcKey} {scriptSrcValue}";
    }

    protected virtual string ContentSecurityPolicyValuesToCSPString(bool ignoreScriptSrc = false)
    {
        if (Options.Value.ContentSecurityPolicyValues.Any())
        {
            return string.Join("; ",
                Options.Value.ContentSecurityPolicyValues.WhereIf(ignoreScriptSrc, x => x.Key != ScriptSrcKey)
                    .Select(x => $"{x.Key} {string.Join(" ", x.Value)}"));
        }

        return DefaultValue;
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