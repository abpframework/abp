using System;
using Medallion.Threading;

namespace Volo.Abp.DistributedLocking;

public static class AbpDistributedLockHandleExtensions
{
    public static IDistributedSynchronizationHandle ToDistributedSynchronizationHandle(
        this IAbpDistributedLockHandle handle)
    {
        return handle.As<MedallionAbpDistributedLockHandle>().Handle;
    }
}
