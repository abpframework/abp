using Microsoft.Extensions.Options;
using Volo.Abp.Dapr;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.DistributedLocking.Dapr;

[Dependency(ReplaceServices = true)]
public class DaprAbpDistributedLock : IAbpDistributedLock, ITransientDependency
{
    protected IAbpDaprClientFactory DaprClientFactory { get; }
    protected AbpDistributedLockDaprOptions DistributedLockDaprOptions { get; }
    protected IDistributedLockKeyNormalizer DistributedLockKeyNormalizer { get; }
    
    public DaprAbpDistributedLock(
        IAbpDaprClientFactory daprClientFactory,
        IOptions<AbpDistributedLockDaprOptions> distributedLockDaprOptions,
        IDistributedLockKeyNormalizer distributedLockKeyNormalizer)
    {
        DaprClientFactory = daprClientFactory;
        DistributedLockKeyNormalizer = distributedLockKeyNormalizer;
        DistributedLockDaprOptions = distributedLockDaprOptions.Value;
    }
    
    public async Task<IAbpDistributedLockHandle?> TryAcquireAsync(
        string name,
        TimeSpan timeout = default,
        CancellationToken cancellationToken = default)
    {
        if (timeout == default)
        {
            timeout = DistributedLockDaprOptions.DefaultTimeout;
        }
        
        name = DistributedLockKeyNormalizer.NormalizeKey(name);

        var daprClient = DaprClientFactory.Create();
        var lockResponse = await daprClient.Lock(
            DistributedLockDaprOptions.StoreName, 
            name, 
            DistributedLockDaprOptions.Owner ?? Guid.NewGuid().ToString(),
            (int)timeout.TotalSeconds,
            cancellationToken);

        if (lockResponse == null || !lockResponse.Success)
        {
            return null;
        }

        return new DaprAbpDistributedLockHandle(lockResponse);
    }
}