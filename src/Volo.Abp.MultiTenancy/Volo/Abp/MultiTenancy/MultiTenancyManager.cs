using System;
using System.Collections.Generic;
using Volo.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class MultiTenancyManager : IMultiTenancyManager, IScopedDependency
    {
        public TenantInfo CurrentTenant => GetCurrentTenant();

        private readonly IAmbientTenantAccessor _ambientTenantAccessor;
        private readonly IEnumerable<ITenantResolver> _currentTenantResolvers;

        public MultiTenancyManager(IAmbientTenantAccessor ambientTenantAccessor, IEnumerable<ITenantResolver> currentTenantResolvers)
        {
            _ambientTenantAccessor = ambientTenantAccessor;
            _currentTenantResolvers = currentTenantResolvers;
        }

        protected virtual TenantInfo GetCurrentTenant()
        {
            if (_ambientTenantAccessor.AmbientTenant != null)
            {
                return _ambientTenantAccessor.AmbientTenant.Tenant;
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
            var oldValue = _ambientTenantAccessor.AmbientTenant;

            _ambientTenantAccessor.AmbientTenant = new AmbientTenantInfo(tenantInfo);

            return new DisposeAction(() =>
            {
                _ambientTenantAccessor.AmbientTenant = oldValue;
            });
        }
    }
}
