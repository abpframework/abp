using System;
using System.IO;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// A readable, non-seekable stream whose length/position probes fail with an
/// <see cref="IOException"/> — a legal stream shape the optional probes must tolerate.
/// </summary>
public sealed class FakeIoFailingLengthStream : Stream
{
    private readonly Stream _inner;

    public FakeIoFailingLengthStream(Stream inner)
    {
        _inner = inner;
    }

    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => throw new IOException("The length is not available!");

    public override long Position
    {
        get => throw new IOException("The position is not available!");
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
        if (disposing)
        {
            _inner.Dispose();
        }

        base.Dispose(disposing);
    }
}
