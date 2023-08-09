using System;
using System.Threading;
using System.Threading.Tasks;
using Medallion.Threading;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.DistributedLocking;

[Dependency(ReplaceServices = true)]
public class MedallionAbpDistributedLock : IAbpDistributedLock, ITransientDependency
{
    protected IDistributedLockProvider DistributedLockProvider { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    
    protected IDistributedLockKeyNormalizer DistributedLockKeyNormalizer { get; }

    public MedallionAbpDistributedLock(
        IDistributedLockProvider distributedLockProvider,
        ICancellationTokenProvider cancellationTokenProvider,
        IDistributedLockKeyNormalizer distributedLockKeyNormalizer)
    {
        DistributedLockProvider = distributedLockProvider;
        CancellationTokenProvider = cancellationTokenProvider;
        DistributedLockKeyNormalizer = distributedLockKeyNormalizer;
    }

    public async Task<IAbpDistributedLockHandle?> TryAcquireAsync(
        string name,
        TimeSpan timeout = default,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        var key = DistributedLockKeyNormalizer.NormalizeKey(name);
        
        CancellationTokenProvider.FallbackToProvider(cancellationToken);

        var handle = await DistributedLockProvider.TryAcquireLockAsync(
            key,
            timeout,
            CancellationTokenProvider.FallbackToProvider(cancellationToken)
        );
        
        if (handle == null)
        {
            return null;
        }

        return new MedallionAbpDistributedLockHandle(handle);
    }
}
