using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class CurrentTenant : ICurrentTenant, ITransientDependency
    {
        public bool IsAvailable => Id.HasValue;

        public Guid? Id => GetCurrentTenant()?.Id;

        public string Name => GetCurrentTenant()?.Name;

        public ConnectionStrings ConnectionStrings => GetCurrentTenant()?.ConnectionStrings;

        private readonly TenantScopeProvider _tenantScopeProvider;
        private readonly ITenantStore _tenantStore;
        private readonly ILogger<CurrentTenant> _logger;
        private readonly ITenantResolver _tenantResolver;

        public CurrentTenant(
            TenantScopeProvider tenantScopeProvider,
            ITenantStore tenantStore,
            ILogger<CurrentTenant> logger,
            ITenantResolver tenantResolver)
        {
            _tenantScopeProvider = tenantScopeProvider;
            _tenantStore = tenantStore;
            _logger = logger;
            _tenantResolver = tenantResolver;
        }

        public IDisposable Change(Guid? id)
        {
            if (id == null)
            {
                return _tenantScopeProvider.EnterScope(null);
            }

            var tenant = _tenantStore.Find(id.Value);
            if (tenant == null)
            {
                throw new AbpException("There is no tenant with given tenant id: " + id.Value);
            }

            return _tenantScopeProvider.EnterScope(tenant);
        }

        public IDisposable Change(string name)
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

        [CanBeNull]
        protected virtual TenantInfo GetCurrentTenant()
        {
            if (_tenantScopeProvider.CurrentScope != null)
            {
                return _tenantScopeProvider.CurrentScope.Tenant;
            }

            //TODO: Get from ICurrentUser before resolvers and fail if resolvers find a different tenant!

            return ResolveTenant();
        }

        [CanBeNull]
        protected virtual TenantInfo ResolveTenant()
        {
            var tenantIdOrName = _tenantResolver.ResolveTenantIdOrName();
            if (tenantIdOrName == null)
            {
                return null;
            }

            TenantInfo tenant;

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
