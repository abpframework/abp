using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.DistributedLocking;

public class LocalAbpDistributedLock : IAbpDistributedLock, ISingletonDependency
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _localSyncObjects = new();
    protected IDistributedLockKeyNormalizer DistributedLockKeyNormalizer { get; }

    public LocalAbpDistributedLock(IDistributedLockKeyNormalizer distributedLockKeyNormalizer)
    {
        DistributedLockKeyNormalizer = distributedLockKeyNormalizer;
    }
    
    public async Task<IAbpDistributedLockHandle> TryAcquireAsync(
        string name,
        TimeSpan timeout = default,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        var key = DistributedLockKeyNormalizer.NormalizeKey(name);
        
        var semaphore = _localSyncObjects.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

        if (!await semaphore.WaitAsync(timeout, cancellationToken))
        {
            return null;
        }

        return new LocalAbpDistributedLockHandle(semaphore);
    }
}
