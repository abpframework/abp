using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class CookieTenantResolver : HttpTenantResolverBase
    {
        protected override string GetTenantIdFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
        {
            return httpContext.Request.Cookies[context.GetAspNetCoreMultiTenancyOptions().TenantIdKey];
        }
    }
}