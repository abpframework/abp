using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// A scoped service that only implements <see cref="IAsyncDisposable"/>: a synchronous
/// dispose of the owning scope would throw for such a service.
/// </summary>
public class FakeAsyncOnlyDisposableService : IScopedDependency, IAsyncDisposable
{
    private static int _asyncDisposedCount;

    public static int AsyncDisposedCount => Volatile.Read(ref _asyncDisposedCount);

    public ValueTask DisposeAsync()
    {
        Interlocked.Increment(ref _asyncDisposedCount);
        return default;
    }
}
