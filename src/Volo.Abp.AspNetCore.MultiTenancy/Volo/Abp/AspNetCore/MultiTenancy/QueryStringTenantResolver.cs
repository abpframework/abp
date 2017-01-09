using Microsoft.AspNetCore.Http;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class QueryStringTenantResolver : HttpTenantResolverBase
    {
        protected override string GetTenantIdFromHttpContextOrNull(HttpContext httpContext)
        {
            if (!httpContext.Request.QueryString.HasValue)
            {
                return null;
            }

            return httpContext.Request.Query[Options.TenantIdKey];
        }
    }
}
