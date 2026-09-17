using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.DistributedLocking;

public class LocalAbpDistributedLock : IAbpDistributedLock, ISingletonDependency
{
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
        var disposable = await KeyedLock.TryLockAsync(key, timeout, cancellationToken);
        if (disposable == null)
        {
            return null;
        }
        return new LocalAbpDistributedLockHandle(disposable);
    }
}
