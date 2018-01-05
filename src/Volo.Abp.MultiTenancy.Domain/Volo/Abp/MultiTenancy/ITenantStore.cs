using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantStore
    {
        [CanBeNull]
        Tenant Find(string name);

        [CanBeNull]
        Tenant Find(Guid id);
    }
}