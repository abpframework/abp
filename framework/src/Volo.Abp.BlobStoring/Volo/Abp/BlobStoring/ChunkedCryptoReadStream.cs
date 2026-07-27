using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// A read-only, non-seekable stream that serves output produced chunk by chunk.
/// </summary>
internal abstract class ChunkedCryptoReadStream : SequentialReadStream
{
    private readonly long? _length;
    private byte[]? _outputBuffer;
    private int _outputBufferPosition;
    private long _position;
    private bool _finished;

    protected ChunkedCryptoReadStream(long? length = null)
    {
        _length = length;
    }

    public override long Length => _length ?? throw new NotSupportedException();

    // Some storage SDKs (like AWS S3) compute the upload size as Length - Position,
    // so the getter reports the number of bytes served so far instead of throwing.
    public override long Position
    {
        get => _position;
        set => throw new NotSupportedException();
    }

    protected sealed override int ReadCore(byte[] buffer, int offset, int count)
    {
        while (true)
        {
            var copiedCount = TryCopyFromOutputBuffer(buffer, offset, count);
            if (copiedCount > 0 || _finished)
            {
                return copiedCount;
            }

            SetOutputBuffer(ProduceNext());
        }
    }

    protected sealed override async Task<int> ReadCoreAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        while (true)
        {
            var copiedCount = TryCopyFromOutputBuffer(buffer, offset, count);
            if (copiedCount > 0 || _finished)
            {
                return copiedCount;
            }

            SetOutputBuffer(await ProduceNextAsync(cancellationToken));
        }
    }

    /// <summary>
    /// Produces the next output bytes, or null when there is no more output.
    /// </summary>
    protected abstract byte[]? ProduceNext();

    protected abstract Task<byte[]?> ProduceNextAsync(CancellationToken cancellationToken);

    internal void EnsureReadToAuthenticatedEndCore()
    {
        // A previous read that faulted the stream (for example an authentication failure
        // a contributor swallowed) must not be recovered by re-entering ProduceNext here
        EnsureCanServe();
        try
        {
            if (IsAtAuthenticatedEnd())
            {
                return;
            }

            // Producing the next record either verifies the terminal record (null) or
            // returns another content chunk, which means the consumer stopped early
            ThrowIfMoreContent(ProduceNext());
        }
        catch
        {
            // Fault the stream so the failure can not be swallowed by reading again
            MarkFaulted();
            throw;
        }
    }

    internal async ValueTask EnsureReadToAuthenticatedEndCoreAsync(CancellationToken cancellationToken)
    {
        EnsureCanServe();
        // A token cancelled before any I/O leaves the stream untouched, so it can stay
        // healthy for a retry (the same rule the normal read path applies). Once a read has
        // started, any failure must fault: a mid-read cancellation already consumed and
        // discarded bytes of the non-seekable cipher stream, so a retry that resumed from the
        // middle of the terminal record would misreport a valid BLOB as corrupt
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            if (IsAtAuthenticatedEnd())
            {
                return;
            }

            ThrowIfMoreContent(await ProduceNextAsync(cancellationToken));
        }
        catch
        {
            MarkFaulted();
            throw;
        }
    }

    private bool IsAtAuthenticatedEnd()
    {
        if (_finished)
        {
            return true;
        }

        if (_outputBuffer != null && _outputBufferPosition < _outputBuffer.Length)
        {
            throw new AbpException(
                "The encrypted BLOB was not read to its authenticated end, so its completeness can not be verified " +
                "(a content-pipeline contributor stopped reading the content before the end).");
        }

        return false;
    }

    private void ThrowIfMoreContent(byte[]? next)
    {
        if (next != null)
        {
            throw new AbpException(
                "The encrypted BLOB was not read to its authenticated end, so its completeness can not be verified " +
                "(a content-pipeline contributor stopped reading the content before the end).");
        }

        SetOutputBuffer(null);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && _outputBuffer != null)
        {
            Array.Clear(_outputBuffer, 0, _outputBuffer.Length);
            _outputBuffer = null;
        }

        base.Dispose(disposing);
    }

    private int TryCopyFromOutputBuffer(byte[] buffer, int offset, int count)
    {
        if (_outputBuffer == null || _outputBufferPosition >= _outputBuffer.Length)
        {
            return 0;
        }

        var toCopy = Math.Min(count, _outputBuffer.Length - _outputBufferPosition);
        Array.Copy(_outputBuffer, _outputBufferPosition, buffer, offset, toCopy);
        _outputBufferPosition += toCopy;
        _position += toCopy;
        return toCopy;
    }

    private void SetOutputBuffer(byte[]? outputBuffer)
    {
        _outputBuffer = outputBuffer;
        _outputBufferPosition = 0;

        if (outputBuffer == null)
        {
            _finished = true;
        }
    }
}
