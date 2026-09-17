using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring.Aws;

/// <summary>
/// The unseekable multipart path of the AWS SDK ignores AutoCloseStream and
/// disposes its input; this wrapper protects the ownership of the wrapped stream.
/// </summary>
internal sealed class LeaveOpenStreamWrapper : Stream
{
    private readonly Stream _inner;

    public LeaveOpenStreamWrapper(Stream inner)
    {
        _inner = inner;
    }

    public override bool CanRead => _inner.CanRead;
    public override bool CanSeek => _inner.CanSeek;
    public override bool CanWrite => false;

    // The SDK computes the optional content length from Length/Position, but only
    // handles NotSupportedException; translate an IOException of a probe, so an
    // unknown length stays "unknown" instead of failing the upload
    public override long Length
    {
        get
        {
            try
            {
                return _inner.Length;
            }
            catch (IOException ex)
            {
                throw new NotSupportedException("The length of the stream is not available!", ex);
            }
        }
    }

    public override long Position
    {
        get
        {
            try
            {
                return _inner.Position;
            }
            catch (IOException ex)
            {
                throw new NotSupportedException("The position of the stream is not available!", ex);
            }
        }
        set => _inner.Position = value;
    }

    public override void Flush()
    {
    }

    public override int Read(byte[] buffer, int offset, int count) => _inner.Read(buffer, offset, count);

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
#if NETSTANDARD2_0
        return _inner.ReadAsync(buffer, offset, count, cancellationToken);
#else
        // The SDK reads over this (old) overload; dispatch it over the modern one,
        // so a source that only implements ReadAsync(Memory<byte>) keeps working
        return _inner.ReadAsync(buffer.AsMemory(offset, count), cancellationToken).AsTask();
#endif
    }

#if !NETSTANDARD2_0
    public override int Read(Span<byte> buffer) => _inner.Read(buffer);

    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        return _inner.ReadAsync(buffer, cancellationToken);
    }
#endif

    public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);
    public override void SetLength(long value) => throw new NotSupportedException();
    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    // Disposing the wrapper must not dispose the wrapped stream
}
