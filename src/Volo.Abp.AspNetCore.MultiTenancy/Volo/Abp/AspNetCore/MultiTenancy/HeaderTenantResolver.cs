using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.MultiTenancy;
using Volo.ExtensionMethods.Collections.Generic;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class HeaderTenantResolver : HttpTenantResolverBase
    {
        protected override string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
        {
            if (httpContext.Request == null || httpContext.Request.Headers.IsNullOrEmpty())
            {
                return null;
            }

            var tenantIdKey = context.GetAspNetCoreMultiTenancyOptions().TenantIdKey;

            var tenantIdHeader = httpContext.Request.Headers[tenantIdKey];
            if (tenantIdHeader == string.Empty || tenantIdHeader.Count < 1)
            {
                return null;
            }

            if (tenantIdHeader.Count > 1)
            {
                context.ServiceProvider.GetRequiredService<ILogger<HeaderTenantResolver>>().LogWarning(
                    $"HTTP request includes more than one {tenantIdKey} header value. First one will be used. All of them: {tenantIdHeader.JoinAsString(", ")}"
                    );
            }

            return tenantIdHeader.First();
        }
    }
}