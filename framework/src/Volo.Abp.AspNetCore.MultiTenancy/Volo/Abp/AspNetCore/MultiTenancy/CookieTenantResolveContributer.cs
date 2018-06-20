using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class CookieTenantResolveContributer : HttpTenantResolveContributerBase
    {
        protected override string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
        {
            return httpContext.Request?.Cookies[context.GetAspNetCoreMultiTenancyOptions().TenantKey];
        }
    }
}