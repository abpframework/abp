using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// Wraps the stream with a pass-through wrapper that fails on Dispose, so tests
/// can verify the best-effort cleanup of the pipeline.
/// </summary>
public class FakeDisposeThrowingPipelineContributor : IBlobPipelineContributor, ITransientDependency
{
    public Task OnSavingAsync(BlobPipelineContext context)
    {
        context.BlobStream = new DisposeThrowingStream(context.BlobStream);
        return Task.CompletedTask;
    }

    public Task OnGettingAsync(BlobPipelineContext context)
    {
        return Task.CompletedTask;
    }

    private sealed class DisposeThrowingStream : Stream
    {
        private readonly Stream _inner;

        public DisposeThrowingStream(Stream inner)
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
            throw new IOException("Injected dispose failure!");
        }
    }
}
