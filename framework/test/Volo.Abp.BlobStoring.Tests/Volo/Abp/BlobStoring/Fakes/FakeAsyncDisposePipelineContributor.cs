#nullable enable
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// Wraps the content with a stream whose cleanup only happens in DisposeAsync,
/// so tests can verify the pipeline releases its streams asynchronously — on the
/// save side and also when the caller disposes the returned stream synchronously.
/// </summary>
public class FakeAsyncDisposePipelineContributor : IBlobPipelineContributor, ITransientDependency
{
    public static AsyncDisposeOnlyStream? LastSaveStream { get; private set; }

    public static AsyncDisposeOnlyStream? LastGetStream { get; private set; }

    public static AsyncDisposeOnlyStream? IntermediateSaveStream { get; private set; }

    public Task OnSavingAsync(BlobPipelineContext context)
    {
        // Two replacements in one call: the intermediate stream must also be collected
        context.BlobStream = IntermediateSaveStream = new AsyncDisposeOnlyStream(context.BlobStream, ownsInner: false);
        context.BlobStream = LastSaveStream = new AsyncDisposeOnlyStream(context.BlobStream, ownsInner: false);
        return Task.CompletedTask;
    }

    public Task OnGettingAsync(BlobPipelineContext context)
    {
        // The get-side contract: the wrapper owns the received stream
        context.BlobStream = LastGetStream = new AsyncDisposeOnlyStream(context.BlobStream, ownsInner: true);
        return Task.CompletedTask;
    }

    public sealed class AsyncDisposeOnlyStream : Stream
    {
        private readonly Stream _inner;
        private readonly bool _ownsInner;

        public bool AsyncDisposed { get; private set; }

        public AsyncDisposeOnlyStream(Stream inner, bool ownsInner)
        {
            _inner = inner;
            _ownsInner = ownsInner;
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

        public override int Read(byte[] buffer, int offset, int count) => _inner.Read(buffer, offset, count);

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            return _inner.ReadAsync(buffer, cancellationToken);
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        // The cleanup only happens asynchronously; the synchronous Dispose is a no-op
        public override async ValueTask DisposeAsync()
        {
            AsyncDisposed = true;
            if (_ownsInner)
            {
                await _inner.DisposeAsync();
            }

            await base.DisposeAsync();
        }
    }
}
