using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantScopeProvider
    {
        [CanBeNull]
        TenantScope CurrentScope { get; }

        IDisposable EnterScope([CanBeNull] TenantInfo tenant);
    }
}