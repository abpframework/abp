using System;
using System.Collections.Generic;
using Volo.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class MultiTenancyManager : IMultiTenancyManager, IScopedDependency
    {
        public TenantInfo CurrentTenant => GetCurrentTenant();

        private readonly ITenantScopeProvider _tenantScopeProvider;
        private readonly IEnumerable<ITenantResolver> _currentTenantResolvers;

        public MultiTenancyManager(
            ITenantScopeProvider tenantScopeProvider, 
            IEnumerable<ITenantResolver> currentTenantResolvers)
        {
            _tenantScopeProvider = tenantScopeProvider;
            _currentTenantResolvers = currentTenantResolvers;
        }

        protected virtual TenantInfo GetCurrentTenant()
        {
            if (_tenantScopeProvider.CurrentScope != null)
            {
                return _tenantScopeProvider.CurrentScope.Tenant;
            }

            var context = new CurrentTenantResolveContext();

            foreach (var currentTenantResolver in _currentTenantResolvers)
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
