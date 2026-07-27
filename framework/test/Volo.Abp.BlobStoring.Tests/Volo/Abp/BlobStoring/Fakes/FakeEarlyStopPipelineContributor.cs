using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// On saving, prepends the 4-byte content length; on getting, returns a wrapper that
/// stops at that declared length — reproducing a contributor that reaches its own EOF
/// before the decryption stream's authenticated end.
/// </summary>
public class FakeEarlyStopPipelineContributor : IBlobPipelineContributor, ITransientDependency
{
    public async Task OnSavingAsync(BlobPipelineContext context)
    {
        using var content = new MemoryStream();
        await context.BlobStream.CopyToAsync(content, 81920, context.CancellationToken);

        var output = new MemoryStream();
        var lengthPrefix = BitConverter.GetBytes((int)content.Length);
        output.Write(lengthPrefix, 0, lengthPrefix.Length);
        content.Position = 0;
        await content.CopyToAsync(output, 81920, context.CancellationToken);
        output.Position = 0;
        context.BlobStream = output;
    }

    public async Task OnGettingAsync(BlobPipelineContext context)
    {
        var lengthPrefix = new byte[4];
        await ReadExactlyAsync(context.BlobStream, lengthPrefix, context.CancellationToken);
        var contentLength = BitConverter.ToInt32(lengthPrefix, 0);
        context.BlobStream = new LengthLimitedStream(context.BlobStream, contentLength);
    }

    private static async Task ReadExactlyAsync(Stream stream, byte[] buffer, CancellationToken cancellationToken)
    {
        var total = 0;
        while (total < buffer.Length)
        {
            var read = await stream.ReadAsync(buffer.AsMemory(total, buffer.Length - total), cancellationToken);
            if (read == 0)
            {
                throw new AbpException("Unexpected end of stream while reading the length prefix!");
            }

            total += read;
        }
    }

    private sealed class LengthLimitedStream : Stream
    {
        private readonly Stream _inner;
        private long _remaining;

        public LengthLimitedStream(Stream inner, long length)
        {
            _inner = inner;
            _remaining = length;
        }

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

        // Stops at the declared length without reading the rest of the inner stream
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_remaining <= 0)
            {
                return 0;
            }

            var toRead = (int)Math.Min(count, _remaining);
            var read = _inner.Read(buffer, offset, toRead);
            _remaining -= read;
            return read;
        }

        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (_remaining <= 0)
            {
                return 0;
            }

            var toRead = (int)Math.Min(buffer.Length, _remaining);
            var read = await _inner.ReadAsync(buffer.Slice(0, toRead), cancellationToken);
            _remaining -= read;
            return read;
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _inner.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
