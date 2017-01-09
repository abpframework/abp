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

            ResolveFromHttpContext(context, httpContext);
        }

        private void ResolveFromHttpContext(ITenantResolveContext context, HttpContext httpContext)
        {
            var tenantId = GetTenantIdFromHttpContextOrNull(context, httpContext);
            if (!tenantId.IsNullOrEmpty())
            {
                context.Tenant = new TenantInfo(tenantId);
            }
        }

        protected abstract string GetTenantIdFromHttpContextOrNull(ITenantResolveContext context,HttpContext httpContext);
    }
}