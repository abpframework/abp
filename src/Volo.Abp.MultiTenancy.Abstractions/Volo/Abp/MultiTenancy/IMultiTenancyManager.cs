using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public interface IMultiTenancyManager
    {
        [CanBeNull]
        TenantInfo CurrentTenant { get; }

        IDisposable ChangeTenant(Guid? tenantId);

        IDisposable ChangeTenant([CanBeNull] string name);
    }
}