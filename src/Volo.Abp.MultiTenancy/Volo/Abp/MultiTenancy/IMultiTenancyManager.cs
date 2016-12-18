using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public interface IMultiTenancyManager
    {
        [CanBeNull]
        TenantInfo CurrentTenant { get; }

        IDisposable ChangeTenant([CanBeNull] TenantInfo tenant);
    }
}