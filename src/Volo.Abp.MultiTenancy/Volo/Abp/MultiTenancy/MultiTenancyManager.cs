using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class MultiTenancyManager : IMultiTenancyManager, ITransientDependency
    {
        public TenantInformation CurrentTenant => GetCurrentTenant();

        private readonly IServiceProvider _serviceProvider;
        private readonly ITenantScopeProvider _tenantScopeProvider;
        private readonly ITenantStore _tenantStore;
        private readonly MultiTenancyOptions _options;
        private readonly ILogger<MultiTenancyManager> _logger;

        public MultiTenancyManager(
            IServiceProvider serviceProvider,
            ITenantScopeProvider tenantScopeProvider,
            IOptions<MultiTenancyOptions> options,
            ITenantStore tenantStore, 
            ILogger<MultiTenancyManager> logger)
        {
            _serviceProvider = serviceProvider;
            _tenantScopeProvider = tenantScopeProvider;
            _tenantStore = tenantStore;
            _logger = logger;
            _options = options.Value;
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

        protected virtual TenantInformation GetCurrentTenant()
        {
            if (_tenantScopeProvider.CurrentScope != null)
            {
                return _tenantScopeProvider.CurrentScope.Tenant;
            }

            //TODO: Get from ICurrentUser before resolvers and fail if resolvers find a different tenant!

            return GetCurrentTenantFromResolvers();
        }

        protected virtual TenantInformation GetCurrentTenantFromResolvers()
        {
            if (!_options.TenantResolvers.Any())
            {
                return null;
            }

            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var context = new TenantResolveContext(serviceScope.ServiceProvider);

                foreach (var tenantResolver in _options.TenantResolvers)
                {
                    tenantResolver.Resolve(context);

                    if (context.HasResolvedTenantOrHost())
                    {
                        if (context.TenantIdOrName == null)
                        {
                            //Resolved host!
                            return null;
                        }

                        var tenant = GetValidatedTenantOrNull(context.TenantIdOrName);
                        if (tenant != null)
                        {
                            return tenant;
                        }

                        _logger.LogWarning($"Resolved tenancy name '{context.TenantIdOrName}' by '{tenantResolver.GetType().FullName}' but could not find in current tenant store.");
                        context.TenantIdOrName = null;
                    }

                    context.Handled = false;
                }

                //Could not find a tenant
                return null;
            }
        }

        [CanBeNull]
        private TenantInformation GetValidatedTenantOrNull([NotNull] string tenantIdOrName)
        {
            Guid tenantId;
            if (Guid.TryParse(tenantIdOrName, out tenantId))
            {
                return _tenantStore.Find(tenantId);
            }

            return _tenantStore.Find(tenantIdOrName);
        }
    }
}
