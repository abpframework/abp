#nullable enable
/*
//Please set the correct connection string in secrets.json and continue the test.
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.BlobStoring.TestObjects;
using Xunit;

namespace Volo.Abp.BlobStoring.Minio;

public class MinioBlobEncryption_Tests : AbpBlobStoringMinioTestBase
{
    private readonly IBlobContainer<TestContainer4> _container4; // UseEncryption("container4-passphrase")

    public MinioBlobEncryption_Tests()
    {
        _container4 = GetRequiredService<IBlobContainer<TestContainer4>>();
    }

    [Fact]
    public async Task Should_Save_And_Get_Encrypted_Blob()
    {
        var blobName = "minio-encrypted-roundtrip";
        var testContent = "minio test content".GetBytes();

        await _container4.SaveAsync(blobName, testContent);

        (await _container4.GetAllBytesAsync(blobName)).SequenceEqual(testContent).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Save_And_Get_Empty_And_Multi_Chunk_Blobs()
    {
        await _container4.SaveAsync("minio-empty", Array.Empty<byte>());
        (await _container4.GetAllBytesAsync("minio-empty")).ShouldBeEmpty();

        // MinIO reads BlobStream.Length before uploading, so this verifies the
        // exact encrypted length calculation against a real object store.
        var largeContent = new byte[3 * 1024 * 1024 + 123]; // Spans many 64 KB chunks
        new Random(42).NextBytes(largeContent);

        await _container4.SaveAsync("minio-large", largeContent);

        (await _container4.GetAllBytesAsync("minio-large")).SequenceEqual(largeContent).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Override_An_Existing_Encrypted_Blob()
    {
        var blobName = "minio-override";
        await _container4.SaveAsync(blobName, "first content".GetBytes());
        await _container4.SaveAsync(blobName, "second content".GetBytes(), overrideExisting: true);

        (await _container4.GetAllBytesAsync(blobName)).ShouldBe("second content".GetBytes());
    }

    [Fact]
    public async Task Should_Support_Exists_And_Delete_For_Encrypted_Blobs()
    {
        var blobName = "minio-exists-delete";
        await _container4.SaveAsync(blobName, "content".GetBytes());

        (await _container4.ExistsAsync(blobName)).ShouldBeTrue();
        (await _container4.DeleteAsync(blobName)).ShouldBeTrue();
        (await _container4.ExistsAsync(blobName)).ShouldBeFalse();
        (await _container4.GetOrNullAsync(blobName)).ShouldBeNull();
    }

    [Fact]
    public async Task Should_Reject_Non_Seekable_Source_Because_Minio_Requires_The_Length()
    {
        // The MinIO provider reads BlobStream.Length; for a non-seekable source the
        // encrypted length is unknown, so saving fails (same as without encryption).
        await Assert.ThrowsAsync<NotSupportedException>(async () =>
        {
            await _container4.SaveAsync("minio-non-seekable", new NonSeekableStream("content".GetBytes()));
        });
    }

    private sealed class NonSeekableStream : Stream
    {
        private readonly MemoryStream _stream;

        public NonSeekableStream(byte[] bytes)
        {
            _stream = new MemoryStream(bytes);
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
            return _stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
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
*/
