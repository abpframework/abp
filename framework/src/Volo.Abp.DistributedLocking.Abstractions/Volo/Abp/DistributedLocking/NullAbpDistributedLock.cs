using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.DistributedLocking;

/// <summary>
/// This implementation of <see cref="IAbpDistributedLock"/> does not provide any distributed locking functionality.
/// Useful in scenarios where distributed locking is not required or during testing.
/// </summary>
public class NullAbpDistributedLock : IAbpDistributedLock
{
    public Task<IAbpDistributedLockHandle?> TryAcquireAsync(string name, TimeSpan timeout = default, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IAbpDistributedLockHandle?>(new LocalAbpDistributedLockHandle(NullDisposable.Instance));
    }
}
