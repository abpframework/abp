using System;
using System.Threading;
using Volo.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class AsyncLocalTenantScopeProvider : ITenantScopeProvider, IScopedDependency
    {
        public TenantScope CurrentScope
        {
            get { return _tenant.Value; }
            private set { _tenant.Value = value; }
        }

        private readonly AsyncLocal<TenantScope> _tenant;

        public AsyncLocalTenantScopeProvider()
        {
            _tenant = new AsyncLocal<TenantScope>();
        }

        public IDisposable EnterScope(TenantInfo tenantInfo)
        {
            var parentScope = CurrentScope;
            CurrentScope = new TenantScope(tenantInfo);
            return new DisposeAction(() =>
            {
                CurrentScope = parentScope;
            });
        }
    }
}