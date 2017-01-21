using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public interface IMultiTenancyManager
    {
        [CanBeNull]
        TenantInformation CurrentTenant { get; }

        IDisposable ChangeTenant(Guid? tenantId);

        IDisposable ChangeTenant([CanBeNull] string name);

        IDisposable ChangeToHost();
    }
}