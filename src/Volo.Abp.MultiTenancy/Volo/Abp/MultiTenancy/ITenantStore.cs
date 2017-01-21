using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantStore
    {
        [CanBeNull]
        TenantInformation Find(string name);

        [CanBeNull]
        TenantInformation Find(Guid id);
    }
}