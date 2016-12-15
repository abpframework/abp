using System;
using Microsoft.Extensions.Options;
using Volo.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class MultiTenancyManager : IMultiTenancyManager, IScopedDependency
    {
        public TenantInfo CurrentTenant => GetCurrentTenant();

        private readonly ITenantScopeProvider _tenantScopeProvider;
        private readonly MultiTenancyOptions _options;

        public MultiTenancyManager(
            ITenantScopeProvider tenantScopeProvider,
            IOptions<MultiTenancyOptions> options)
        {
            _tenantScopeProvider = tenantScopeProvider;
            _options = options.Value;
        }

        protected virtual TenantInfo GetCurrentTenant()
        {
            if (_tenantScopeProvider.CurrentScope != null)
            {
                return _tenantScopeProvider.CurrentScope.Tenant;
            }

            var context = new CurrentTenantResolveContext();

            foreach (var currentTenantResolver in _options.TenantResolvers)
            {
                currentTenantResolver.Resolve(context);
                if (context.Handled)
                {
                    break;
                }
            }

            return context.Tenant;
        }

        public IDisposable ChangeTenant(TenantInfo tenantInfo)
        {
            return _tenantScopeProvider.EnterScope(tenantInfo);
        }
    }
}
