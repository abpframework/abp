using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// Prepends a marker while saving and verifies/strips it while getting,
/// so tests can observe the raw stored form and the execution order.
/// </summary>
public abstract class FakeMarkerPipelineContributorBase : IBlobPipelineContributor
{
    private readonly byte[] _marker;

    protected FakeMarkerPipelineContributorBase(string marker)
    {
        _marker = Encoding.UTF8.GetBytes(marker);
    }

    public Task OnSavingAsync(BlobPipelineContext context)
    {
        context.BlobStream = new MarkerPrependingStream(_marker, context.BlobStream);
        return Task.CompletedTask;
    }

    public Task OnGettingAsync(BlobPipelineContext context)
    {
        context.BlobStream = new MarkerStrippingStream(_marker, context.BlobStream);
        return Task.CompletedTask;
    }

    private sealed class MarkerPrependingStream : Stream
    {
        private readonly byte[] _marker;
        private readonly Stream _inner;
        private int _markerPosition;

        public MarkerPrependingStream(byte[] marker, Stream inner)
        {
            _marker = marker;
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

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_markerPosition < _marker.Length)
            {
                var toCopy = Math.Min(count, _marker.Length - _markerPosition);
                Array.Copy(_marker, _markerPosition, buffer, offset, toCopy);
                _markerPosition += toCopy;
                return toCopy;
            }

            return _inner.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        // The save-side contract: the wrapper leaves the received stream open
    }

    private sealed class MarkerStrippingStream : Stream
    {
        private readonly byte[] _marker;
        private readonly Stream _inner;
        private bool _markerConsumed;

        public MarkerStrippingStream(byte[] marker, Stream inner)
        {
            _marker = marker;
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

        public override int Read(byte[] buffer, int offset, int count)
        {
            ConsumeMarker();
            return _inner.Read(buffer, offset, count);
        }

        private void ConsumeMarker()
        {
            if (_markerConsumed)
            {
                return;
            }

            _markerConsumed = true;

            var markerBytes = new byte[_marker.Length];
            var readCount = 0;
            while (readCount < markerBytes.Length)
            {
                var read = _inner.Read(markerBytes, readCount, markerBytes.Length - readCount);
                if (read <= 0)
                {
                    break;
                }

                readCount += read;
            }

            if (readCount != _marker.Length || !((ReadOnlySpan<byte>)markerBytes).SequenceEqual(_marker))
            {
                throw new AbpException($"The expected content marker '{Encoding.UTF8.GetString(_marker)}' was not found!");
            }
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            // The get-side contract: the wrapper owns the received stream
            if (disposing)
            {
                _inner.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
