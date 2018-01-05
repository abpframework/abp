using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    //TODO: This is very similar to ITenantScopeProvider. Consider to unify them!

    public interface IMultiTenancyManager
    {
        [CanBeNull]
        TenantInformation CurrentTenant { get; }

        IDisposable ChangeTenant(Guid? tenantId);

        IDisposable ChangeTenant([CanBeNull] string name);
    }
}