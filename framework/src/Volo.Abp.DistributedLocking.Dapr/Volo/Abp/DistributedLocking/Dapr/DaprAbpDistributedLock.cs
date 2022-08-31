using Microsoft.Extensions.Options;
using Volo.Abp.Dapr;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.DistributedLocking.Dapr;

[Dependency(ReplaceServices = true)]
public class DaprAbpDistributedLock : IAbpDistributedLock, ITransientDependency
{
    protected AbpDaprClientFactory DaprClientFactory { get; }
    protected AbpDistributedLockDaprOptions DistributedLockDaprOptions { get; }
    protected AbpDaprOptions DaprOptions { get; }
    
    public DaprAbpDistributedLock(
        AbpDaprClientFactory daprClientFactory,
        IOptions<AbpDistributedLockDaprOptions> distributedLockDaprOptions,
        IOptions<AbpDaprOptions> daprOptions)
    {
        DaprClientFactory = daprClientFactory;
        DaprOptions = daprOptions.Value;
        DistributedLockDaprOptions = distributedLockDaprOptions.Value;
    }
    
    public async Task<IAbpDistributedLockHandle> TryAcquireAsync(
        string name,
        TimeSpan timeout = default,
        CancellationToken cancellationToken = default)
    {
        if (timeout == default)
        {
            timeout = DistributedLockDaprOptions.DefaultTimeout;
        }
        
        var daprClient = await DaprClientFactory.CreateAsync();

        var lockResponse = await daprClient.Lock(
            DistributedLockDaprOptions.StoreName, 
            name, 
            DaprOptions.AppId,
            (int)timeout.TotalSeconds,
            cancellationToken);

        if (lockResponse == null || !lockResponse.Success)
        {
            return null;
        }

        return new DaprAbpDistributedLockHandle(lockResponse);
    }
}