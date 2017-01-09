using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;
using Volo.ExtensionMethods;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public abstract class HttpTenantResolverBase : ITenantResolver
    {
        protected AspNetCoreMultiTenancyOptions Options { get; private set; }

        public virtual void Resolve(ITenantResolveContext context)
        {
            Options = context.ServiceProvider.GetRequiredService<IOptions<AspNetCoreMultiTenancyOptions>>().Value;

            var httpContext = context.GetHttpContext();
            if (httpContext == null)
            {
                return;
            }

            ResolveFromHttpContext(context, httpContext);
        }

        private void ResolveFromHttpContext(ITenantResolveContext context, HttpContext httpContext)
        {
            var tenantId = GetTenantIdFromHttpContextOrNull(httpContext);
            if (!tenantId.IsNullOrEmpty())
            {
                context.Tenant = new TenantInfo(tenantId);
            }
        }

        protected abstract string GetTenantIdFromHttpContextOrNull(HttpContext httpContext);
    }
}