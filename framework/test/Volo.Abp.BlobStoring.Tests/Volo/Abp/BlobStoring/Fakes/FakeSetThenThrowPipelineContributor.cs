#nullable enable
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// Replaces the stream and then fails, so tests can verify that the
/// already-created stream is not leaked.
/// </summary>
public class FakeSetThenThrowPipelineContributor : IBlobPipelineContributor, ITransientDependency
{
    public static TrackableStream? LastCreatedStream { get; private set; }

    public Task OnSavingAsync(BlobPipelineContext context)
    {
        context.BlobStream = LastCreatedStream = new TrackableStream(context.BlobStream);
        throw new InvalidOperationException("This contributor fails after replacing the stream!");
    }

    public Task OnGettingAsync(BlobPipelineContext context)
    {
        return Task.CompletedTask;
    }

    public sealed class TrackableStream : Stream
    {
        private readonly Stream _inner;

        public bool Disposed { get; private set; }

        public TrackableStream(Stream inner)
        {
            _inner = inner;
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
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            Disposed = true;
            base.Dispose(disposing);
        }
    }
}
