using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
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
            // Try to delete the tenant's cookie if it does not exist or is inactive.
            var tenantResolveResult = context.RequestServices.GetRequiredService<ITenantResolveResultAccessor>().Result;
            if (tenantResolveResult != null &&
                tenantResolveResult.AppliedResolvers.Contains(CookieTenantResolveContributor.ContributorName))
            {
                var options = context.RequestServices.GetRequiredService<IOptions<AbpAspNetCoreMultiTenancyOptions>>().Value;
                AbpMultiTenancyCookieHelper.SetTenantCookie(context, null, options.TenantKey);
            }

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; ;
            context.Response.ContentType = "text/html";

            var message = exception.Message;
            var details = exception is BusinessException businessException ? businessException.Details : string.Empty;

            await context.Response.WriteAsync($"<html lang=\"{CultureInfo.CurrentCulture.Name}\"><body>\r\n");
            await context.Response.WriteAsync($"<h3>{message}</h3>{details}<br>\r\n");
            await context.Response.WriteAsync("</body></html>\r\n");

            // Note the 500 spaces are to work around an IE 'feature'
            await context.Response.WriteAsync(new string(' ', 500));

            return true;
        };
    }
}
