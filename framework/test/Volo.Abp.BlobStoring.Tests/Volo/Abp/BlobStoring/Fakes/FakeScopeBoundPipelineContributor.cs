using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

public class FakeScopeBoundPipelineContributor : IBlobPipelineContributor, IScopedDependency, IDisposable
{
    private bool _isDisposed;

    public Task<Stream> OnSaveAsync(BlobPipelineSaveArgs args)
    {
        return Task.FromResult<Stream>(new ScopeBoundStream(args.BlobStream, this));
    }

    public Task<Stream> OnGetAsync(BlobPipelineGetArgs args)
    {
        return Task.FromResult<Stream>(new ScopeBoundStream(args.BlobStream, this));
    }

    public void Dispose()
    {
        _isDisposed = true;
    }

    private sealed class ScopeBoundStream : Stream
    {
        private readonly Stream _stream;
        private readonly FakeScopeBoundPipelineContributor _owner;

        public ScopeBoundStream(Stream stream, FakeScopeBoundPipelineContributor owner)
        {
            _stream = stream;
            _owner = owner;
        }

        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => _stream.CanSeek;
        public override bool CanWrite => false;
        public override long Length => _stream.Length;

        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_owner._isDisposed)
            {
                throw new ObjectDisposedException(nameof(FakeScopeBoundPipelineContributor));
            }

            return _stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _stream.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
