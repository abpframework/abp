using System;
using System.Threading;
using Volo.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class AsyncLocalAmbientTenantScopeProvider : IAmbientTenantScopeProvider, IScopedDependency
    {
        public AmbientTenantScope CurrentScope
        {
            get { return _tenant.Value; }
            set { _tenant.Value = value; }
        }

        private readonly AsyncLocal<AmbientTenantScope> _tenant;

        public AsyncLocalAmbientTenantScopeProvider()
        {
            _tenant = new AsyncLocal<AmbientTenantScope>();
        }

        public IDisposable CreateScope(TenantInfo tenantInfo)
        {
            var parentScope = CurrentScope;
            CurrentScope = new AmbientTenantScope(tenantInfo);
            return new DisposeAction(() =>
            {
                CurrentScope = parentScope;
            });
        }
    }
}