using System;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantScopeProvider
    {
        TenantScope CurrentScope { get; }

        IDisposable EnterScope(TenantInfo tenantInfo);
    }
}