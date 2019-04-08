using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class MultiTenancyMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ITenantResolver _tenantResolver;
        private readonly ITenantStore _tenantStore;
        private readonly ICurrentTenant _currentTenant;
        private readonly ITenantResolveResultAccessor _tenantResolveResultAccessor;

        public MultiTenancyMiddleware(
            RequestDelegate next,
            ITenantResolver tenantResolver, 
            ITenantStore tenantStore, 
            ICurrentTenant currentTenant, 
            ITenantResolveResultAccessor tenantResolveResultAccessor)
        {
            _next = next;
            _tenantResolver = tenantResolver;
            _tenantStore = tenantStore;
            _currentTenant = currentTenant;
            _tenantResolveResultAccessor = tenantResolveResultAccessor;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var resolveResult = _tenantResolver.ResolveTenantIdOrName();
            _tenantResolveResultAccessor.Result = resolveResult;

            TenantConfiguration tenant = null;
            if (resolveResult.TenantIdOrName != null)
            {
                tenant = await FindTenantAsync(resolveResult.TenantIdOrName);
                if (tenant == null)
                {
                    //TODO: A better exception?
                    throw new AbpException(
                        "There is no tenant with given tenant id or name: " + resolveResult.TenantIdOrName
                    );
                }
            }

            using (_currentTenant.Change(tenant?.Id, tenant?.Name))
            {
                await _next(httpContext);
            }
        }

        private async Task<TenantConfiguration> FindTenantAsync(string tenantIdOrName)
        {
            if (Guid.TryParse(tenantIdOrName, out var parsedTenantId))
            {
                return await _tenantStore.FindAsync(parsedTenantId);
            }
            else
            {
                return await _tenantStore.FindAsync(tenantIdOrName);
            }
        }
    }
}
