using Dapr.Client;

namespace Volo.Abp.DistributedLocking.Dapr;

public class DaprAbpDistributedLockHandle : IAbpDistributedLockHandle
{
    protected TryLockResponse LockResponse { get; }
    
    public DaprAbpDistributedLockHandle(TryLockResponse lockResponse)
    {
        LockResponse = lockResponse;
    }
    
    public async ValueTask DisposeAsync()
    {
        await LockResponse.DisposeAsync();
    }
}