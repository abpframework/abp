using System;
using JetBrains.Annotations;
using Volo.Abp.Threading;

namespace Volo.Abp.MultiTenancy
{
    public static class TenantStoreExtensions
    {
        [CanBeNull]
        public static TenantInfo Find(this ITenantStore tenantStore, string name)
        {
            return AsyncHelper.RunSync(() => tenantStore.FindAsync(name));
        }

        [CanBeNull]
        public static TenantInfo Find(this ITenantStore tenantStore, Guid id)
        {
            return AsyncHelper.RunSync(() => tenantStore.FindAsync(id));
        }
    }
}