using System;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    //TODO: This is very similar to ITenantScopeProvider. Consider to unify them!

    public class MultiTenancyManager : IMultiTenancyManager, ITransientDependency
    {
        public Tenant CurrentTenant => GetCurrentTenant();

        private readonly ITenantScopeProvider _tenantScopeProvider;
        private readonly ITenantStore _tenantStore;
        private readonly ILogger<MultiTenancyManager> _logger;
        private readonly ITenantResolver _tenantResolver;

        public MultiTenancyManager(
            ITenantScopeProvider tenantScopeProvider,
            ITenantStore tenantStore,
            ILogger<MultiTenancyManager> logger,
            ITenantResolver tenantResolver)
        {
            _tenantScopeProvider = tenantScopeProvider;
            _tenantStore = tenantStore;
            _logger = logger;
            _tenantResolver = tenantResolver;
        }

        public IDisposable ChangeTenant(Guid? tenantId)
        {
            if (tenantId == null)
            {
                return _tenantScopeProvider.EnterScope(null);
            }

            var tenant = _tenantStore.Find(tenantId.Value);
            if (tenant == null)
            {
                throw new AbpException("There is no tenant with given tenant id: " + tenantId.Value);
            }

            return _tenantScopeProvider.EnterScope(tenant);
        }

        public IDisposable ChangeTenant(string name)
        {
            if (name == null)
            {
                return _tenantScopeProvider.EnterScope(null);
            }

            var tenant = _tenantStore.Find(name);
            if (tenant == null)
            {
                throw new AbpException("There is no tenant with given tenant name: " + name);
            }

            return _tenantScopeProvider.EnterScope(tenant);
        }

        protected virtual Tenant GetCurrentTenant()
        {
            if (_tenantScopeProvider.CurrentScope != null)
            {
                return _tenantScopeProvider.CurrentScope.Tenant;
            }

            //TODO: Get from ICurrentUser before resolvers and fail if resolvers find a different tenant!

            return ResolveTenant();
        }

        protected virtual Tenant ResolveTenant()
        {
            var tenantIdOrName = _tenantResolver.ResolveTenantIdOrName();
            if (tenantIdOrName == null)
            {
                return null;
            }

            Tenant tenant;

            //Try to find by id
            if (Guid.TryParse(tenantIdOrName, out var tenantId))
            {
                tenant = _tenantStore.Find(tenantId);
                if (tenant != null)
                {
                    return tenant;
                }
            }

            //Try to find by name
            tenant = _tenantStore.Find(tenantIdOrName);
            if (tenant != null)
            {
                return tenant;
            }

            //Could not found!
            _logger.LogWarning($"Resolved tenancy id or name '{tenantIdOrName}' but could not find in the tenant store.");
            return null;
        }
    }
}
