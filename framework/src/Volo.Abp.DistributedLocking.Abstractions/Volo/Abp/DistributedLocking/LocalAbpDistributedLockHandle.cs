using System;
using System.Threading.Tasks;

namespace Volo.Abp.DistributedLocking;

public class LocalAbpDistributedLockHandle : IAbpDistributedLockHandle
{
    private readonly IDisposable _disposable;

    public LocalAbpDistributedLockHandle(IDisposable disposable)
    {
        _disposable = disposable;
    }

    public ValueTask DisposeAsync()
    {
        _disposable.Dispose();
        return default;
    }
}
