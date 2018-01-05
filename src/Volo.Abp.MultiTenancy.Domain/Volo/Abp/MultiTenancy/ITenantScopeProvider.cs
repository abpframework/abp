using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantScopeProvider
    {
        /// <summary>
        /// Can be null for host.
        /// </summary>
        [CanBeNull]
        TenantScope CurrentScope { get; }

        IDisposable EnterScope([CanBeNull] Tenant tenant);
    }
}