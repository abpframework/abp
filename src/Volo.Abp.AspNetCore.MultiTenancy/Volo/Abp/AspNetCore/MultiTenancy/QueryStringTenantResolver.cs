using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class QueryStringTenantResolver : HttpTenantResolverBase
    {
        protected override string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
        {
            if (!httpContext.Request.QueryString.HasValue)
            {
                return null;
            }

            return httpContext.Request.Query[context.GetAspNetCoreMultiTenancyOptions().TenantIdKey];
        }
    }
}
