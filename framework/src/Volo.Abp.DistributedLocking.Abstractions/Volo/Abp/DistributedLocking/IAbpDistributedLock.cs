using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.DistributedLocking;

public interface IAbpDistributedLock
{
    /// <summary>
    /// Tries to acquire a named lock.
    /// Returns a disposable object to release the lock.
    /// It is suggested to use this method within a using block. 
    /// Returns null if the lock could not be handled.
    /// </summary>
    /// <param name="name">The name of the lock</param>
    /// <param name="timeout">How long to wait before giving up on the acquisition attempt. Defaults to 0</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [ItemCanBeNull]
    Task<IAbpDistributedLockHandle> TryAcquireAsync(
        [NotNull] string name,
        TimeSpan timeout = default,
        CancellationToken cancellationToken = default
    );
}
