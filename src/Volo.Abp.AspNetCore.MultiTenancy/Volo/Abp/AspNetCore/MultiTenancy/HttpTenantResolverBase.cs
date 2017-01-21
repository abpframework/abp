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
            var tenantIdOrName = GetTenantIdOrNameFromHttpContextOrNull(context, httpContext);
            if (!tenantIdOrName.IsNullOrEmpty())
            {
                context.TenantIdOrName = tenantIdOrName;
            }
        }

        protected abstract string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context,HttpContext httpContext);
    }
}