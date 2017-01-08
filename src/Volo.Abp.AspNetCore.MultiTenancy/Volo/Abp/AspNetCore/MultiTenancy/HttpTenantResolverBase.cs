using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiTenancy;
using Volo.ExtensionMethods;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public abstract class HttpTenantResolverBase : ITenantResolver
    {
        public virtual void Resolve(ITenantResolveContext context)
        {
            var httpContext = context.GetHttpContext();
            if (httpContext == null)
            {
                return;
            }

            var tenantId = GetTenantIdFromHttpContextOrNull(httpContext);
            if (!tenantId.IsNullOrEmpty())
            {
                context.Tenant = new TenantInfo(tenantId);
            }
        }

        protected abstract string GetTenantIdFromHttpContextOrNull(HttpContext httpContext);
    }
}