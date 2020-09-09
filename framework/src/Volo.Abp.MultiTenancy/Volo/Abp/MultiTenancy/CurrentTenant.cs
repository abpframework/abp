using System;
using System.Security.Principal;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.MultiTenancy
{
    public class CurrentTenant : ICurrentTenant, ITransientDependency
    {
        public virtual bool IsAvailable => Id.HasValue;

        public virtual Guid? Id => _currentTenantAccessor.Current?.TenantId;

        public virtual Guid? ImpersonatorId => _principalAccessor.Principal?.FindTenantImpersonatorId();

        public string Name => _currentTenantAccessor.Current?.Name;

        private readonly ICurrentTenantAccessor _currentTenantAccessor;
        private readonly ICurrentPrincipalAccessor _principalAccessor;

        public CurrentTenant(ICurrentTenantAccessor currentTenantAccessor, ICurrentPrincipalAccessor principalAccessor)
        {
            _currentTenantAccessor = currentTenantAccessor;
            _principalAccessor = principalAccessor;
        }

        public IDisposable Change(Guid? id, string name = null)
        {
            return SetCurrent(id, name);
        }

        private IDisposable SetCurrent(Guid? tenantId, string name = null)
        {
            var parentScope = _currentTenantAccessor.Current;
            _currentTenantAccessor.Current = new BasicTenantInfo(tenantId, name);
            return new DisposeAction(() =>
            {
                _currentTenantAccessor.Current = parentScope;
            });
        }
    }
}
