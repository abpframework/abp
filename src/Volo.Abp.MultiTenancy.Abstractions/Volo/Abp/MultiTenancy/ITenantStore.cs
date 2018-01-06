using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantStore
    {
        [CanBeNull]
        TenantInfo Find(string name);

        [CanBeNull]
        TenantInfo Find(Guid id);
    }
}