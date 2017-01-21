using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class RouteTenantResolver : HttpTenantResolverBase
    {
        protected override string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
        {
            var tenantId = httpContext.GetRouteValue(context.GetAspNetCoreMultiTenancyOptions().TenantIdKey);
            if (tenantId == null)
            {
                return null;
            }

            return Convert.ToString(tenantId);
        }
    }
}