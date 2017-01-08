using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class MultiTenancyManager : IMultiTenancyManager, ITransientDependency
    {
        public TenantInfo CurrentTenant => GetCurrentTenant();

        private readonly IServiceProvider _serviceProvider;
        private readonly ITenantScopeProvider _tenantScopeProvider;
        private readonly MultiTenancyOptions _options;

        public MultiTenancyManager(
            IServiceProvider serviceProvider,
            ITenantScopeProvider tenantScopeProvider,
            IOptions<MultiTenancyOptions> options)
        {
            _serviceProvider = serviceProvider;
            _tenantScopeProvider = tenantScopeProvider;
            _options = options.Value;
        }

        protected virtual TenantInfo GetCurrentTenant()
        {
            if (_tenantScopeProvider.CurrentScope != null)
            {
                return _tenantScopeProvider.CurrentScope.Tenant;
            }

            return GetCurrentTenantFromResolvers();
        }

        protected virtual TenantInfo GetCurrentTenantFromResolvers()
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

                    //TODO: Validate TenantId by a TenantStore (TenantId can be TenancyName, so also normalize it by tenant store)

                    if (context.IsHandled())
                    {
                        break;
                    }

                    context.Handled = null;
                }

                return context.Tenant;
            }
        }

        public IDisposable ChangeTenant(TenantInfo tenant)
        {
            return _tenantScopeProvider.EnterScope(tenant);
        }
    }
}
