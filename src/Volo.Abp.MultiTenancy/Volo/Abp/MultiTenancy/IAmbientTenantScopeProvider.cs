using System;

namespace Volo.Abp.MultiTenancy
{
    public interface IAmbientTenantScopeProvider
    {
        AmbientTenantScope CurrentScope { get; set; }

        IDisposable CreateScope(TenantInfo tenantInfo);
    }
}