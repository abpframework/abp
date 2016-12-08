using System;

namespace Volo.Abp.MultiTenancy
{
    public interface IMultiTenancyManager
    {
        TenantInfo CurrentTenant { get; }

        IDisposable ChangeTenant(TenantInfo tenantInfo);
    }
}