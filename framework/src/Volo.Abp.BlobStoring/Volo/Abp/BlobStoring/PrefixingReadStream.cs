using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Threading;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// Serves the already-consumed prefix bytes first, then the rest of the underlying stream.
/// </summary>
internal sealed class PrefixingReadStream : SequentialReadStream
{
    private readonly byte[] _prefix;
    private readonly Stream _stream;
    private readonly long? _length;
    private int _prefixPosition;

    public PrefixingReadStream(byte[] prefix, Stream stream)
    {
        _prefix = prefix;
        _stream = stream;

        // What this stream serves is the prefix plus whatever remains of the underlying
        // stream from its current position — not the underlying total length, which
        // would overstate it when the provider stream did not start at position 0
        try
        {
            _length = _prefix.Length + (_stream.Length - _stream.Position);
        }
        catch (Exception ex) when (ex is NotSupportedException || ex is IOException)
        {
            _length = null;
        }
    }

    public override bool CanRead => !IsDisposed && _stream.CanRead;

    // Legacy plaintext BLOBs had a usable Length before encryption was enabled; it is
    // known when the underlying stream reports both its length and position. Position
    // reports the bytes served, so Length - Position stays meaningful for length-aware
    // consumers (like re-encrypting the legacy content)
    public override long Length => _length ?? throw new NotSupportedException();

    public override long Position
    {
        get => _position;
        set => throw new NotSupportedException();
    }

    private long _position;

    protected override int ReadCore(byte[] buffer, int offset, int count)
    {
        var prefixReadCount = TryCopyFromPrefix(buffer, offset, count);
        if (prefixReadCount > 0)
        {
            _position += prefixReadCount;
            return prefixReadCount;
        }

        var readCount = _stream.Read(buffer, offset, count);
        _position += readCount;
        return readCount;
    }

    protected override async Task<int> ReadCoreAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        var prefixReadCount = TryCopyFromPrefix(buffer, offset, count);
        if (prefixReadCount > 0)
        {
            _position += prefixReadCount;
            return prefixReadCount;
        }

#if NETSTANDARD2_0
        var readCount = await _stream.ReadAsync(buffer, offset, count, cancellationToken);
#else
        // The modern overload dispatches correctly for streams that only
        // implement ReadAsync(Memory<byte>)
        var readCount = await _stream.ReadAsync(buffer.AsMemory(offset, count), cancellationToken);
#endif
        _position += readCount;
        return readCount;
    }

    private int TryCopyFromPrefix(byte[] buffer, int offset, int count)
    {
        if (_prefixPosition >= _prefix.Length)
        {
            return 0;
        }

        var readCount = Math.Min(count, _prefix.Length - _prefixPosition);
        Array.Copy(_prefix, _prefixPosition, buffer, offset, readCount);
        _prefixPosition += readCount;
        return readCount;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && !IsDisposed)
        {
            IsDisposed = true;
            try
            {
#if NETSTANDARD2_0
                // Stream has no DisposeAsync on netstandard2.0, but the provider stream
                // may still implement IAsyncDisposable for its async-only cleanup
                if (_stream is IAsyncDisposable asyncDisposable)
                {
                    AsyncHelper.RunSync(() => asyncDisposable.DisposeAsync().AsTask());
                }
                else
                {
                    _stream.Dispose();
                }
#else
                // Also covers a provider stream that only implements DisposeAsync
                AsyncHelper.RunSync(() => _stream.DisposeAsync().AsTask());
#endif
            }
            finally
            {
                base.Dispose(disposing);
            }

            return;
        }

        base.Dispose(disposing);
    }

#if !NETSTANDARD2_0
    public override async ValueTask DisposeAsync()
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            try
            {
                await _stream.DisposeAsync();
            }
            finally
            {
                await base.DisposeAsync();
            }

            return;
        }

        await base.DisposeAsync();
    }
#endif
}
