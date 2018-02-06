using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class CurrentTenant : ICurrentTenant, ITransientDependency
    {
        public virtual bool IsAvailable => Id.HasValue;

        public virtual Guid? Id => _currentTenantIdAccessor.Current?.TenantId;

        private readonly ICurrentTenantIdAccessor _currentTenantIdAccessor;

        public CurrentTenant(ICurrentTenantIdAccessor currentTenantIdAccessor)
        {
            _currentTenantIdAccessor = currentTenantIdAccessor;
        }

        public IDisposable Change(Guid? id)
        {
            return SetCurrent(id);
        }

        public IDisposable Clear() //TODO: Remove
        {
            return Change(null);
        }

        private IDisposable SetCurrent(Guid? tenantId)
        {
            var parentScope = _currentTenantIdAccessor.Current;
            _currentTenantIdAccessor.Current = new TenantIdWrapper(tenantId);
            return new DisposeAction(() =>
            {
                _currentTenantIdAccessor.Current = parentScope;
            });
        }
    }
}
