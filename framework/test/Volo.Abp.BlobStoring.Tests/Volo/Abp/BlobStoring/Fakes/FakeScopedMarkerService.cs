using System;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// A scoped service used by <see cref="FakeScopedXorPipelineContributor"/> to prove
/// that the contributor scope stays alive while the returned stream is being read.
/// </summary>
public class FakeScopedMarkerService : IScopedDependency, IDisposable
{
    private static int _disposedCount;

    public static int DisposedCount => Volatile.Read(ref _disposedCount);

    private bool _disposed;

    public byte Transform(byte value)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(FakeScopedMarkerService));
        }

        return (byte)(value ^ 0x5A);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            Interlocked.Increment(ref _disposedCount);
        }
    }
}
