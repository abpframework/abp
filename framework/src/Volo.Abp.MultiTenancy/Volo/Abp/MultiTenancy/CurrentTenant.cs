using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class CurrentTenant : ICurrentTenant, ITransientDependency
    {
        public virtual bool IsAvailable => Id.HasValue;

        public virtual Guid? Id => _currentTenantIdAccessor.Current?.TenantId;

        public string Name => _currentTenantIdAccessor.Current?.Name;

        private readonly ICurrentTenantIdAccessor _currentTenantIdAccessor;

        public CurrentTenant(ICurrentTenantIdAccessor currentTenantIdAccessor)
        {
            _currentTenantIdAccessor = currentTenantIdAccessor;
        }

        public IDisposable Change(Guid? id, string name = null)
        {
            return SetCurrent(id, name);
        }

        private IDisposable SetCurrent(Guid? tenantId, string name = null)
        {
            var parentScope = _currentTenantIdAccessor.Current;
            _currentTenantIdAccessor.Current = new BasicTenantInfo(tenantId, name);
            return new DisposeAction(() =>
            {
                _currentTenantIdAccessor.Current = parentScope;
            });
        }
    }
}
