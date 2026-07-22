using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// Serves the already-consumed prefix bytes first, then the rest of the underlying stream.
/// </summary>
internal sealed class PrefixingReadStream : SequentialReadStream
{
    private readonly byte[] _prefix;
    private readonly Stream _stream;
    private int _prefixPosition;

    public PrefixingReadStream(byte[] prefix, Stream stream)
    {
        _prefix = prefix;
        _stream = stream;
    }

    public override bool CanRead => _stream.CanRead;

    // Legacy plaintext BLOBs had a usable Length before encryption was enabled;
    // keep it available when the underlying stream knows it.
    public override long Length => _stream.CanSeek ? _stream.Length : throw new NotSupportedException();

    protected override int ReadCore(byte[] buffer, int offset, int count)
    {
        var prefixReadCount = TryCopyFromPrefix(buffer, offset, count);
        if (prefixReadCount > 0)
        {
            return prefixReadCount;
        }

        return _stream.Read(buffer, offset, count);
    }

    protected override async Task<int> ReadCoreAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        var prefixReadCount = TryCopyFromPrefix(buffer, offset, count);
        if (prefixReadCount > 0)
        {
            return prefixReadCount;
        }

        return await _stream.ReadAsync(buffer, offset, count, cancellationToken);
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
                _stream.Dispose();
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
