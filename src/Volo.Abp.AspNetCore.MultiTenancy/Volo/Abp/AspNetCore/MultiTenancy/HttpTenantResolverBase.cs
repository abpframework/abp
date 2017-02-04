using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Volo.Abp.MultiTenancy;
using Volo.ExtensionMethods;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public abstract class HttpTenantResolverBase : ITenantResolver
    {
        private readonly ILogger<HttpTenantResolverBase> _logger;

        protected HttpTenantResolverBase(ILogger<HttpTenantResolverBase> logger)
        {
            _logger = logger;
        }

        public virtual void Resolve(ITenantResolveContext context)
        {
            var httpContext = context.GetHttpContext();
            if (httpContext == null)
            {
                return;
            }

            try
            {
                ResolveFromHttpContext(context, httpContext);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.ToString());
            }
        }

        private void ResolveFromHttpContext(ITenantResolveContext context, HttpContext httpContext)
        {
            var tenantIdOrName = GetTenantIdOrNameFromHttpContextOrNull(context, httpContext);
            if (!tenantIdOrName.IsNullOrEmpty())
            {
                context.TenantIdOrName = tenantIdOrName;
            }
        }

        protected abstract string GetTenantIdOrNameFromHttpContextOrNull([NotNull] ITenantResolveContext context, [NotNull] HttpContext httpContext);
    }
}