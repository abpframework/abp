using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.DistributedLocking;

public class LocalAbpDistributedLockHandle : IAbpDistributedLockHandle
{
    private readonly SemaphoreSlim _semaphore;

    public LocalAbpDistributedLockHandle(SemaphoreSlim semaphore)
    {
        _semaphore = semaphore;
    }

    public ValueTask DisposeAsync()
    {
        _semaphore.Release();
        return default;
    }
}
