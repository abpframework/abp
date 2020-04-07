using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class MultiTenancyMiddleware : IMiddleware, ITransientDependency
    {
        private readonly ITenantResolver _tenantResolver;
        private readonly ITenantStore _tenantStore;
        private readonly ICurrentTenant _currentTenant;
        private readonly ITenantResolveResultAccessor _tenantResolveResultAccessor;

        public MultiTenancyMiddleware(
            ITenantResolver tenantResolver,
            ITenantStore tenantStore,
            ICurrentTenant currentTenant,
            ITenantResolveResultAccessor tenantResolveResultAccessor)
        {
            _tenantResolver = tenantResolver;
            _tenantStore = tenantStore;
            _currentTenant = currentTenant;
            _tenantResolveResultAccessor = tenantResolveResultAccessor;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var resolveResult = _tenantResolver.ResolveTenantIdOrName();
            _tenantResolveResultAccessor.Result = resolveResult;

            TenantConfiguration tenant = null;
            if (resolveResult.TenantIdOrName != null)
            {
                tenant = await FindTenantAsync(resolveResult.TenantIdOrName);

                if (tenant == null)
                {
                    throw new BusinessException(
                        code: "Volo.AbpIo.MultiTenancy:010001",
                        message: "Tenant not found!",
                        details: "There is no tenant with the tenant id or name: " + resolveResult.TenantIdOrName
                    );
                }
            }

            using (_currentTenant.Change(tenant?.Id, tenant?.Name))
            {
                await next(context);
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
