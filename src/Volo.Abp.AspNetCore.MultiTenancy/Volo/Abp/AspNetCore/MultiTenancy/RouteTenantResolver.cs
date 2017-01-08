using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class RouteTenantResolver : HttpTenantResolverBase
    {
        protected override string GetTenantIdFromHttpContextOrNull(HttpContext httpContext)
        {
            var tenantId = httpContext.GetRouteValue(AbpAspNetCoreMultiTenancyConsts.TenantIdKey);
            if (tenantId == null)
            {
                return null;
            }

            return Convert.ToString(tenantId);
        }
    }
}