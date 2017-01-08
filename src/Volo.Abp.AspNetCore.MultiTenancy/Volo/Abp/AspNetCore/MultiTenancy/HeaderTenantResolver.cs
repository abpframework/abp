using Microsoft.AspNetCore.Http;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class HeaderTenantResolver : HttpTenantResolverBase
    {
        protected override string GetTenantIdFromHttpContextOrNull(HttpContext httpContext)
        {
            //TODO: Get first one if provided multiple values and write a log
            return httpContext.Request.Headers[AbpAspNetCoreMultiTenancyConsts.TenantIdKey];
        }
    }
}