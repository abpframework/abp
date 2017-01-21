using System.Linq;
using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiTenancy;
using Volo.ExtensionMethods.Collections.Generic;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class HeaderTenantResolver : HttpTenantResolverBase
    {
        protected override string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
        {
            //TODO: Get first one if provided multiple values and write a log
            if (httpContext.Request.Headers.IsNullOrEmpty())
            {
                return null;
            }

            var tenantIdHeader = httpContext.Request.Headers[context.GetAspNetCoreMultiTenancyOptions().TenantIdKey];
            if (tenantIdHeader == string.Empty || tenantIdHeader.Count < 1)
            {
                return null;
            }

            if (tenantIdHeader.Count > 1)
            {
                
            }

            return tenantIdHeader.First();
        }
    }
}