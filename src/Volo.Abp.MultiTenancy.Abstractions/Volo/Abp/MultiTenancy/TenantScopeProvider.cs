using System;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    /* This cass uses TenantScope instead of TenantInfo because being null of CurrentScope is different that being null of CurrentScope.Tenant.
     * A null CurrentScope indicates that we haven't set any scope explicitly (and we can use tenant resolvers to determine the current tenant).
     * A null CurrentScope.Tenant indicates that we have set null tenant value (maybe to switch to host) explicitly.
     * A non-null CurrentScope.Tenant indicates that we have set a tenant value explicitly.
     */

    public class TenantScopeProvider : ISingletonDependency
    {
        public TenantScope CurrentScope
        {
            get => _currentScope.Value;
            private set => _currentScope.Value = value;
        }

        private readonly AsyncLocal<TenantScope> _currentScope;

        public TenantScopeProvider()
        {
            _currentScope = new AsyncLocal<TenantScope>();
        }

        public IDisposable EnterScope(TenantInfo tenant)
        {
            var parentScope = CurrentScope;
            CurrentScope = new TenantScope(tenant);
            return new DisposeAction(() =>
            {
                CurrentScope = parentScope;
            });
        }
    }
}