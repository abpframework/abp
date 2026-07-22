#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.BlobStoring.TestObjects;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.BlobStoring.FileSystem;

public class FileSystemBlobEncryption_Tests : AbpBlobStoringFileSystemTestBase
{
    private readonly IBlobContainer<TestContainer4> _container4; // UseEncryption("container4-passphrase")
    private readonly IBlobContainer<TestContainer5> _container5; // UseEncryption() -> key provider (tenant setting / global options)
    private readonly IBlobContainer<TestContainer6> _container6; // UseEncryption("container6-passphrase", allowLegacyPlaintext: true)
    private readonly IBlobFilePathCalculator _filePathCalculator;
    private readonly IBlobContainerConfigurationProvider _configurationProvider;
    private readonly ICurrentTenant _currentTenant;

    public FileSystemBlobEncryption_Tests()
    {
        _container4 = GetRequiredService<IBlobContainer<TestContainer4>>();
        _container5 = GetRequiredService<IBlobContainer<TestContainer5>>();
        _container6 = GetRequiredService<IBlobContainer<TestContainer6>>();
        _filePathCalculator = GetRequiredService<IBlobFilePathCalculator>();
        _configurationProvider = GetRequiredService<IBlobContainerConfigurationProvider>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public async Task Should_Store_Encrypted_Bytes_On_Disk_And_Read_Them_Back()
    {
        var blobName = "fs-encrypted-roundtrip";
        var testContent = "file system test content".GetBytes();

        await _container4.SaveAsync(blobName, testContent);

        var fileBytes = await File.ReadAllBytesAsync(GetFilePath<TestContainer4>(blobName));
        fileBytes.SequenceEqual(testContent).ShouldBeFalse();
        Encoding.ASCII.GetString(fileBytes.Take(4).ToArray()).ShouldBe("ABPE");

        (await _container4.GetAllBytesAsync(blobName)).SequenceEqual(testContent).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Save_And_Get_Empty_And_Multi_Chunk_Blobs()
    {
        await _container4.SaveAsync("fs-empty", Array.Empty<byte>());
        (await _container4.GetAllBytesAsync("fs-empty")).ShouldBeEmpty();

        var largeContent = new byte[3 * 1024 * 1024 + 123]; // Spans many 64 KB chunks
        new Random(42).NextBytes(largeContent);

        await _container4.SaveAsync("fs-large", largeContent);

        (await _container4.GetAllBytesAsync("fs-large")).SequenceEqual(largeContent).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Override_An_Existing_Encrypted_Blob()
    {
        var blobName = "fs-override";
        await _container4.SaveAsync(blobName, "first content".GetBytes());
        await _container4.SaveAsync(blobName, "second content".GetBytes(), overrideExisting: true);

        (await _container4.GetAllBytesAsync(blobName)).ShouldBe("second content".GetBytes());
    }

    [Fact]
    public async Task Should_Save_From_Async_Only_Source_To_Disk()
    {
        var blobName = "fs-async-only";
        var testContent = new byte[192 * 1024];
        new Random(42).NextBytes(testContent);

        await _container4.SaveAsync(blobName, new AsyncOnlyStream(testContent));

        using var result = await _container4.GetAsync(blobName);
        using var output = new MemoryStream();
        await result.CopyToAsync(output);

        output.ToArray().ShouldBe(testContent);
    }

    [Fact]
    public async Task Should_Read_Legacy_Plaintext_File_When_Allowed()
    {
        var blobName = "fs-legacy";
        var legacyContent = "plaintext file from before encryption".GetBytes();
        WriteRawFile<TestContainer6>(blobName, legacyContent);

        (await _container6.GetAllBytesAsync(blobName)).SequenceEqual(legacyContent).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Reject_Legacy_Plaintext_File_By_Default()
    {
        var blobName = "fs-legacy-rejected";
        WriteRawFile<TestContainer4>(blobName, "plaintext file".GetBytes());

        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            using var stream = await _container4.GetAsync(blobName);
        });
    }

    [Fact]
    public async Task Should_Detect_Tampered_File_On_Disk()
    {
        var blobName = "fs-tampered";
        var content = new byte[128 * 1024];
        new Random(42).NextBytes(content);
        await _container4.SaveAsync(blobName, content);

        var filePath = GetFilePath<TestContainer4>(blobName);
        var fileBytes = await File.ReadAllBytesAsync(filePath);
        fileBytes[100] ^= 0xFF; // Inside the first cipher chunk
        await File.WriteAllBytesAsync(filePath, fileBytes);

        using var stream = await _container4.GetAsync(blobName);
        using var output = new MemoryStream();

        Assert.ThrowsAny<CryptographicException>(() => stream.CopyTo(output));
    }

    [Fact]
    public async Task Should_Detect_Truncated_File_On_Disk()
    {
        var blobName = "fs-truncated";
        await _container4.SaveAsync(blobName, new byte[128 * 1024]);

        var filePath = GetFilePath<TestContainer4>(blobName);
        var fileBytes = await File.ReadAllBytesAsync(filePath);
        Array.Resize(ref fileBytes, fileBytes.Length - 20); // Cut the terminal record
        await File.WriteAllBytesAsync(filePath, fileBytes);

        using var stream = await _container4.GetAsync(blobName);
        using var output = new MemoryStream();

        Should.Throw<AbpException>(() => stream.CopyTo(output));
    }

    [Fact]
    public async Task Should_Fail_Closed_When_File_Magic_Is_Tampered()
    {
        var blobName = "fs-tampered-magic";
        await _container4.SaveAsync(blobName, "secret".GetBytes());

        var filePath = GetFilePath<TestContainer4>(blobName);
        var fileBytes = await File.ReadAllBytesAsync(filePath);
        fileBytes[0] ^= 0xFF;
        await File.WriteAllBytesAsync(filePath, fileBytes);

        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            using var stream = await _container4.GetAsync(blobName);
        });
    }

    [Fact]
    public async Task Should_Support_Exists_And_Delete_For_Encrypted_Blobs()
    {
        var blobName = "fs-exists-delete";
        await _container4.SaveAsync(blobName, "content".GetBytes());

        (await _container4.ExistsAsync(blobName)).ShouldBeTrue();
        (await _container4.DeleteAsync(blobName)).ShouldBeTrue();
        (await _container4.ExistsAsync(blobName)).ShouldBeFalse();
        (await _container4.GetOrNullAsync(blobName)).ShouldBeNull();
    }

    [Fact]
    public async Task Should_Isolate_Tenant_Blobs_In_Separate_Files()
    {
        var blobName = "fs-tenant-isolation";
        var tenant1 = Guid.NewGuid();
        var tenant2 = Guid.NewGuid();

        using (_currentTenant.Change(tenant1))
        {
            await _container5.SaveAsync(blobName, "tenant 1 content".GetBytes());
        }

        using (_currentTenant.Change(tenant2))
        {
            await _container5.SaveAsync(blobName, "tenant 2 content".GetBytes());
        }

        string tenant1Path, tenant2Path;
        using (_currentTenant.Change(tenant1))
        {
            tenant1Path = GetFilePath<TestContainer5>(blobName);
            (await _container5.GetAllBytesAsync(blobName)).ShouldBe("tenant 1 content".GetBytes());
        }

        using (_currentTenant.Change(tenant2))
        {
            tenant2Path = GetFilePath<TestContainer5>(blobName);
            (await _container5.GetAllBytesAsync(blobName)).ShouldBe("tenant 2 content".GetBytes());
        }

        tenant1Path.ShouldNotBe(tenant2Path);
        File.Exists(tenant1Path).ShouldBeTrue();
        File.Exists(tenant2Path).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Reject_A_File_Moved_Between_Tenants()
    {
        var blobName = "fs-moved-between-tenants";
        var tenant1 = Guid.NewGuid();
        var tenant2 = Guid.NewGuid();

        string tenant1Path, tenant2Path;
        using (_currentTenant.Change(tenant1))
        {
            await _container4.SaveAsync(blobName, "tenant 1 secret".GetBytes());
            tenant1Path = GetFilePath<TestContainer4>(blobName);
        }

        using (_currentTenant.Change(tenant2))
        {
            tenant2Path = GetFilePath<TestContainer4>(blobName);
        }

        // Same container passphrase for both tenants: only the identity binding
        // makes the copied file unreadable at the new location.
        Directory.CreateDirectory(Path.GetDirectoryName(tenant2Path)!);
        File.Copy(tenant1Path, tenant2Path);

        using (_currentTenant.Change(tenant2))
        {
            using var stream = await _container4.GetAsync(blobName);
            using var output = new MemoryStream();

            Assert.ThrowsAny<CryptographicException>(() => stream.CopyTo(output));
        }
    }

    [Fact]
    public async Task Should_Use_Global_PassPhrase_On_Disk_Without_Tenant()
    {
        var blobName = "fs-global-key";
        await _container5.SaveAsync(blobName, "global content".GetBytes());

        var fileBytes = await File.ReadAllBytesAsync(GetFilePath<TestContainer5>(blobName));
        fileBytes[6].ShouldBe((byte)BlobEncryptionKeySource.Global);

        (await _container5.GetAllBytesAsync(blobName)).ShouldBe("global content".GetBytes());
    }

    [Fact]
    public async Task Should_Retry_And_Produce_A_Complete_File_For_A_Replayable_Source()
    {
        // TestContainer8 is not encrypted, so the seekable source reaches the provider directly
        var container8 = GetRequiredService<IBlobContainer<TestContainer8>>();
        var content = new byte[64 * 1024];
        new Random(42).NextBytes(content);
        var source = new FaultOnceSeekableStream(content);

        await container8.SaveAsync("fs-retry-replayable", source, overrideExisting: true);

        source.FaultsInjected.ShouldBe(1); // First attempt failed, the retry succeeded
        (await container8.GetAllBytesAsync("fs-retry-replayable")).SequenceEqual(content).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Not_Retry_A_Non_Replayable_Encrypted_Save()
    {
        // The encrypting wrapper is not seekable, so a mid-write failure must not be retried
        var content = new byte[64 * 1024];
        new Random(42).NextBytes(content);
        var source = new FaultOnceSeekableStream(content, reportSeekable: false);

        await Assert.ThrowsAsync<IOException>(async () =>
        {
            await _container4.SaveAsync("fs-retry-non-replayable", source, overrideExisting: true);
        });

        source.FaultsInjected.ShouldBe(1); // No second attempt
    }

    private sealed class FaultOnceSeekableStream : Stream
    {
        private readonly MemoryStream _stream;
        private readonly bool _reportSeekable;
        private bool _faulted;

        public int FaultsInjected { get; private set; }

        public FaultOnceSeekableStream(byte[] bytes, bool reportSeekable = true)
        {
            _stream = new MemoryStream(bytes);
            _reportSeekable = reportSeekable;
        }

        public override bool CanRead => true;
        public override bool CanSeek => _reportSeekable;
        public override bool CanWrite => false;
        public override long Length => _reportSeekable ? _stream.Length : throw new NotSupportedException();

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
            return ReadCore(() => _stream.Read(buffer, offset, count));
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(ReadCore(() => _stream.Read(buffer, offset, count)));
        }

        private int ReadCore(Func<int> read)
        {
            // Fail once in the middle of the content
            if (!_faulted && _stream.Position >= _stream.Length / 2)
            {
                _faulted = true;
                FaultsInjected++;
                throw new IOException("Injected I/O failure!");
            }

            return read();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _reportSeekable ? _stream.Seek(offset, origin) : throw new NotSupportedException();
        }

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

    private string GetFilePath<TContainer>(string blobName)
    {
        return _filePathCalculator.Calculate(
            new BlobProviderGetArgs(
                BlobContainerNameAttribute.GetContainerName<TContainer>(),
                _configurationProvider.Get<TContainer>(),
                blobName
            )
        );
    }

    private void WriteRawFile<TContainer>(string blobName, byte[] bytes)
    {
        var filePath = GetFilePath<TContainer>(blobName);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllBytes(filePath, bytes);
    }

    private sealed class AsyncOnlyStream : Stream
    {
        private readonly MemoryStream _stream;

        public AsyncOnlyStream(byte[] bytes)
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
            throw new InvalidOperationException("Synchronous reads are not allowed on this stream!");
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken)
        {
            return _stream.ReadAsync(buffer, offset, count, cancellationToken);
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
