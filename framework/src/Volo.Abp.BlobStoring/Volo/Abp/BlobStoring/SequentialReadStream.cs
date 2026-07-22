using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// A read-only, non-seekable, forward-only stream; a failed read faults it permanently.
/// </summary>
internal abstract class SequentialReadStream : Stream
{
    private bool _faulted;

    protected bool IsDisposed { get; set; }

    public override bool CanRead => true;

    public override bool CanSeek => false;

    public override bool CanWrite => false;

    public override long Length => throw new NotSupportedException();

    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public override void Flush()
    {
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        ValidateReadArguments(buffer, offset, count);
        EnsureCanServe();

        if (count == 0)
        {
            return 0;
        }

        try
        {
            return ReadCore(buffer, offset, count);
        }
        catch
        {
            _faulted = true;
            throw;
        }
    }

    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        ValidateReadArguments(buffer, offset, count);
        EnsureCanServe();
        cancellationToken.ThrowIfCancellationRequested();

        if (count == 0)
        {
            return 0;
        }

        try
        {
            return await ReadCoreAsync(buffer, offset, count, cancellationToken);
        }
        catch
        {
            _faulted = true;
            throw;
        }
    }

    protected abstract int ReadCore(byte[] buffer, int offset, int count);

    protected abstract Task<int> ReadCoreAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }

    protected override void Dispose(bool disposing)
    {
        IsDisposed = true;
        base.Dispose(disposing);
    }

    private void EnsureCanServe()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }

        if (_faulted)
        {
            throw new AbpException("The stream can not be read anymore, because a previous read operation has failed!");
        }
    }

    private static void ValidateReadArguments(byte[] buffer, int offset, int count)
    {
        if (buffer == null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

        if (offset < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(offset));
        }

        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        if (buffer.Length - offset < count)
        {
            throw new ArgumentException("The sum of offset and count is larger than the buffer length!");
        }
    }
}
