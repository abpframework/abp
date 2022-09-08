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

    public MedallionAbpDistributedLock(
        IDistributedLockProvider distributedLockProvider,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        DistributedLockProvider = distributedLockProvider;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    public async Task<IAbpDistributedLockHandle> TryAcquireAsync(
        string name,
        TimeSpan timeout = default,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        CancellationTokenProvider.FallbackToProvider(cancellationToken);

        var handle = await DistributedLockProvider.TryAcquireLockAsync(
            name,
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
