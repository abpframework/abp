using System;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
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