using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class AsyncLocalCurrentTenantIdAccessor : ICurrentTenantIdAccessor, ISingletonDependency
    {
        public TenantIdWrapper Current
        {
            get => _currentScope.Value;
            set => _currentScope.Value = value;
        }

        private readonly AsyncLocal<TenantIdWrapper> _currentScope;

        public AsyncLocalCurrentTenantIdAccessor()
        {
            _currentScope = new AsyncLocal<TenantIdWrapper>();
        }
    }
}