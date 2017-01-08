using Microsoft.AspNetCore.Http;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class CookieTenantResolver : HttpTenantResolverBase
    {
        protected override string GetTenantIdFromHttpContextOrNull(HttpContext httpContext)
        {
            return httpContext.Request.Cookies[AbpAspNetCoreMultiTenancyConsts.TenantIdKey];
        }
    }
}