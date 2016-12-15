using System;
using System.Collections.Generic;
using Volo.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class MultiTenancyManager : IMultiTenancyManager, IScopedDependency
    {
        public TenantInfo CurrentTenant => GetCurrentTenant();

        private readonly IAmbientTenantScopeProvider _ambientTenantScopeProvider;
        private readonly IEnumerable<ITenantResolver> _currentTenantResolvers;

        public MultiTenancyManager(
            IAmbientTenantScopeProvider ambientTenantScopeProvider, 
            IEnumerable<ITenantResolver> currentTenantResolvers)
        {
            _ambientTenantScopeProvider = ambientTenantScopeProvider;
            _currentTenantResolvers = currentTenantResolvers;
        }

        protected virtual TenantInfo GetCurrentTenant()
        {
            if (_ambientTenantScopeProvider.CurrentScope != null)
            {
                return _ambientTenantScopeProvider.CurrentScope.Tenant;
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
            return _ambientTenantScopeProvider.EnterScope(tenantInfo);
        }
    }
}
