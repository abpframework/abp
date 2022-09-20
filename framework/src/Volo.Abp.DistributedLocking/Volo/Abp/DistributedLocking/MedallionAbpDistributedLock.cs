using System;
using System.Threading;
using System.Threading.Tasks;
using Medallion.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.DistributedLocking;

[Dependency(ReplaceServices = true)]
public class MedallionAbpDistributedLock : IAbpDistributedLock, ITransientDependency
{
    protected IDistributedLockProvider DistributedLockProvider { get; }

    public MedallionAbpDistributedLock(IDistributedLockProvider distributedLockProvider)
    {
        DistributedLockProvider = distributedLockProvider;
    }

    public async Task<IAbpDistributedLockHandle> TryAcquireAsync(
        string name,
        TimeSpan timeout = default,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var handle = await DistributedLockProvider.TryAcquireLockAsync(name, timeout, cancellationToken);
        if (handle == null)
        {
            return null;
        }

        return new MedallionAbpDistributedLockHandle(handle);
    }
}
