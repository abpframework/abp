using System.Linq;
using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class ClaimsHttpTenantResolveContributer : HttpTenantResolveContributerBase
    {
        protected override void ResolveFromHttpContext(ITenantResolveContext context, HttpContext httpContext)
        {
            if (httpContext.User?.Identity?.IsAuthenticated == true)
            {
                base.ResolveFromHttpContext(context, httpContext);
                context.Handled = true;
            }
        }

        protected override string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
        {
            var tenantKey = context.GetAspNetCoreMultiTenancyOptions().TenantKey;
            return httpContext.User.Claims.FirstOrDefault(c => c.Type == tenantKey)?.Value;
        }
    }
}