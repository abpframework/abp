using System;

namespace Volo.Abp.MultiTenancy
{
    public interface IAmbientTenantScopeProvider
    {
        AmbientTenantScope CurrentScope { get; set; }

        IDisposable EnterScope(TenantInfo tenantInfo);
    }
}