using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncKeyedLock;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.DistributedLocking;

public class LocalAbpDistributedLock : IAbpDistributedLock, ISingletonDependency
{
    private readonly AsyncKeyedLocker<string> _localSyncObjects = new();
    protected IDistributedLockKeyNormalizer DistributedLockKeyNormalizer { get; }

    public LocalAbpDistributedLock(IDistributedLockKeyNormalizer distributedLockKeyNormalizer)
    {
        DistributedLockKeyNormalizer = distributedLockKeyNormalizer;
    }

    public async Task<IAbpDistributedLockHandle?> TryAcquireAsync(
        string name,
        TimeSpan timeout = default,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        var key = DistributedLockKeyNormalizer.NormalizeKey(name);

        var timeoutReleaser = await _localSyncObjects.LockOrNullAsync(key, timeout, cancellationToken);
        if (timeoutReleaser is not null)
        {
            return new LocalAbpDistributedLockHandle(timeoutReleaser);
        }
        return null;
    }
}
