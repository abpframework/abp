using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class TenantConfigurationProvider : ITenantConfigurationProvider, ITransientDependency
    {
        protected virtual ITenantResolver TenantResolver { get; }
        protected virtual ITenantStore TenantStore { get; }
        protected virtual ITenantResolveResultAccessor TenantResolveResultAccessor { get; }

        public TenantConfigurationProvider(
            ITenantResolver tenantResolver,
            ITenantStore tenantStore,
            ITenantResolveResultAccessor tenantResolveResultAccessor)
        {
            TenantResolver = tenantResolver;
            TenantStore = tenantStore;
            TenantResolveResultAccessor = tenantResolveResultAccessor;
        }

        public virtual async Task<TenantConfiguration> GetAsync(bool saveResolveResult = false)
        {
            var resolveResult = await TenantResolver.ResolveTenantIdOrNameAsync();

            if (saveResolveResult)
            {
                TenantResolveResultAccessor.Result = resolveResult;
            }

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

                if (!tenant.IsActive)
                {
                    throw new BusinessException(
                        code: "Volo.AbpIo.MultiTenancy:010002",
                        message: "Tenant not active!",
                        details: "The tenant is no active with the tenant id or name: " + resolveResult.TenantIdOrName
                    );
                }
            }

            return tenant;
        }

        protected virtual async Task<TenantConfiguration> FindTenantAsync(string tenantIdOrName)
        {
            if (Guid.TryParse(tenantIdOrName, out var parsedTenantId))
            {
                return await TenantStore.FindAsync(parsedTenantId);
            }
            else
            {
                return await TenantStore.FindAsync(tenantIdOrName);
            }
        }
    }
}
