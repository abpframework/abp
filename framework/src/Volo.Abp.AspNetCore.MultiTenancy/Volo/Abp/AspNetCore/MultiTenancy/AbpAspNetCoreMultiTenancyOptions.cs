using System;
using System.Globalization;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy;

public class AbpAspNetCoreMultiTenancyOptions
{
    /// <summary>
    /// Default: <see cref="TenantResolverConsts.DefaultTenantKey"/>.
    /// </summary>
    public string TenantKey { get; set; }

    /// <summary>
    /// Return true to stop the pipeline, false to continue.
    /// </summary>
    public Func<HttpContext, Exception, Task<bool>> MultiTenancyMiddlewareErrorPageBuilder { get; set; }

    public AbpAspNetCoreMultiTenancyOptions()
    {
        TenantKey = TenantResolverConsts.DefaultTenantKey;
        MultiTenancyMiddlewareErrorPageBuilder = async (context, exception) =>
        {
            var tenantResolveResult = context.RequestServices.GetRequiredService<ITenantResolveResultAccessor>().Result;
            if (tenantResolveResult != null)
            {
                if (tenantResolveResult.AppliedResolvers.Count == 1 && tenantResolveResult.AppliedResolvers.Contains(CurrentUserTenantResolveContributor.ContributorName))
                {
                    var authenticationType = context.User.Identity?.AuthenticationType;
                    if (authenticationType != null)
                    {
                        var scheme = await context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>().GetHandlerAsync(context, authenticationType);
                        if (scheme is IAuthenticationSignOutHandler signOutHandler)
                        {
                            // Try to delete the authentication's cookie if it does not exist or is inactive.
                            await signOutHandler.SignOutAsync(null);
                        }
                    }
                }

                var options = context.RequestServices.GetRequiredService<IOptions<AbpAspNetCoreMultiTenancyOptions>>().Value;
                if (tenantResolveResult.AppliedResolvers.Contains(CookieTenantResolveContributor.ContributorName) ||
                    context.Request.Cookies.ContainsKey(options.TenantKey))
                {
                    // Try to delete the tenant's cookie if it does not exist or is inactive.
                    AbpMultiTenancyCookieHelper.SetTenantCookie(context, null, options.TenantKey);
                }
            }

            context.Response.Headers.Add("Abp-Tenant-Resolve-Error", exception.Message);
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "text/html";

            var message = exception.Message;
            var details = exception is BusinessException businessException ? businessException.Details : string.Empty;

            await context.Response.WriteAsync($"<html lang=\"{HtmlEncoder.Default.Encode(CultureInfo.CurrentCulture.Name)}\"><body>\r\n");
            await context.Response.WriteAsync($"<h3>{HtmlEncoder.Default.Encode(message)}</h3>{HtmlEncoder.Default.Encode(details)}<br>\r\n");
            await context.Response.WriteAsync("</body></html>\r\n");

            // Note the 500 spaces are to work around an IE 'feature'
            await context.Response.WriteAsync(new string(' ', 500));

            return true;
        };
    }
}
