using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// XOR-transforms the content through a scoped service that is used lazily,
/// while the stream is being read.
/// </summary>
public class FakeScopedXorPipelineContributor : IBlobPipelineContributor, ITransientDependency
{
    private readonly FakeScopedMarkerService _markerService;

    public FakeScopedXorPipelineContributor(FakeScopedMarkerService markerService)
    {
        _markerService = markerService;
    }

    public Task OnSavingAsync(BlobPipelineContext context)
    {
        context.BlobStream = new XorStream(_markerService, context.BlobStream, ownsInner: false);
        return Task.CompletedTask;
    }

    public Task OnGettingAsync(BlobPipelineContext context)
    {
        context.BlobStream = new XorStream(_markerService, context.BlobStream, ownsInner: true);
        return Task.CompletedTask;
    }

    private sealed class XorStream : Stream
    {
        private readonly FakeScopedMarkerService _markerService;
        private readonly Stream _inner;
        private readonly bool _ownsInner;

        public XorStream(FakeScopedMarkerService markerService, Stream inner, bool ownsInner)
        {
            _markerService = markerService;
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

        public override int Read(byte[] buffer, int offset, int count)
        {
            var readCount = _inner.Read(buffer, offset, count);
            for (var i = 0; i < readCount; i++)
            {
                buffer[offset + i] = _markerService.Transform(buffer[offset + i]);
            }

            return readCount;
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing && _ownsInner)
            {
                _inner.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
