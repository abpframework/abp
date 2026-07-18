#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.BlobStoring.Fakes;
using Volo.Abp.BlobStoring.TestObjects;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using Xunit;

namespace Volo.Abp.BlobStoring;

public class BlobContainerEncryption_Tests : AbpBlobStoringTestBase
{
    private readonly IBlobContainer<TestContainer4> _container4; // UseEncryption("container4-passphrase")
    private readonly IBlobContainer<TestContainer5> _container5; // UseEncryption() -> key provider (tenant setting / global options)
    private readonly IBlobContainer<TestContainer6> _container6; // FakeReversingPipelineContributor
    private readonly IBlobContainer<TestContainer7> _container7; // Scope-bound lazy contributor
    private readonly FakeInMemoryBlobProvider _provider;
    private readonly IBlobEncryptionService _encryptionService;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISettingDefinitionManager _settingDefinitionManager;

    public BlobContainerEncryption_Tests()
    {
        _container4 = GetRequiredService<IBlobContainer<TestContainer4>>();
        _container5 = GetRequiredService<IBlobContainer<TestContainer5>>();
        _container6 = GetRequiredService<IBlobContainer<TestContainer6>>();
        _container7 = GetRequiredService<IBlobContainer<TestContainer7>>();
        _provider = GetRequiredService<FakeInMemoryBlobProvider>();
        _encryptionService = GetRequiredService<IBlobEncryptionService>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
        _settingDefinitionManager = GetRequiredService<ISettingDefinitionManager>();
    }

    [Fact]
    public async Task Should_Save_Encrypted_And_Get_Decrypted_Blob()
    {
        var blobName = "test-blob-encrypted-1";
        var testContent = "test content".GetBytes();

        await _container4.SaveAsync(blobName, testContent);

        var rawBytes = GetRawBytes<TestContainer4>(blobName);
        rawBytes.ShouldNotBeNull();
        rawBytes!.SequenceEqual(testContent).ShouldBeFalse();
        Encoding.ASCII.GetString(rawBytes.Take(4).ToArray()).ShouldBe("ABPE");

        var result = await _container4.GetAllBytesAsync(blobName);
        result.SequenceEqual(testContent).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Encrypt_With_Tenant_Specific_PassPhrase()
    {
        var tenantId = Guid.NewGuid();
        var blobName = "test-blob-encrypted-tenant";
        var testContent = "test content".GetBytes();

        using (_currentTenant.Change(tenantId))
        {
            await _container5.SaveAsync(blobName, testContent);
        }

        var rawBytes = GetRawBytes<TestContainer5>(blobName);
        rawBytes.ShouldNotBeNull();

        var decryptedBytes = Decrypt(rawBytes!, FakeTenantPassPhraseSettingValueProvider.GetPassPhrase(tenantId));
        decryptedBytes.SequenceEqual(testContent).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Produce_Different_CipherText_For_Different_Tenants()
    {
        var testContent = "test content".GetBytes();

        var tenantId1 = Guid.NewGuid();
        using (_currentTenant.Change(tenantId1))
        {
            await _container5.SaveAsync("test-blob-tenant-1", testContent);
        }

        var tenantId2 = Guid.NewGuid();
        using (_currentTenant.Change(tenantId2))
        {
            await _container5.SaveAsync("test-blob-tenant-2", testContent);
        }

        var rawBytes1 = GetRawBytes<TestContainer5>("test-blob-tenant-1");
        var rawBytes2 = GetRawBytes<TestContainer5>("test-blob-tenant-2");

        rawBytes1.ShouldNotBeNull();
        rawBytes2.ShouldNotBeNull();
        rawBytes1!.SequenceEqual(rawBytes2!).ShouldBeFalse();

        Decrypt(rawBytes1, FakeTenantPassPhraseSettingValueProvider.GetPassPhrase(tenantId1))
            .SequenceEqual(testContent).ShouldBeTrue();
        Decrypt(rawBytes2, FakeTenantPassPhraseSettingValueProvider.GetPassPhrase(tenantId2))
            .SequenceEqual(testContent).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Fall_Back_To_Global_PassPhrase_Without_Tenant()
    {
        var blobName = "test-blob-encrypted-global";
        var testContent = "test content".GetBytes();

        await _container5.SaveAsync(blobName, testContent);

        var rawBytes = GetRawBytes<TestContainer5>(blobName);
        rawBytes.ShouldNotBeNull();

        var decryptedBytes = Decrypt(rawBytes!, "default-global-passphrase");
        decryptedBytes.SequenceEqual(testContent).ShouldBeTrue();

        (await _container5.GetAllBytesAsync(blobName)).SequenceEqual(testContent).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Apply_Custom_Pipeline_Contributor()
    {
        var blobName = "test-blob-reversed";
        var testContent = "test content".GetBytes();

        await _container6.SaveAsync(blobName, testContent);

        var rawBytes = GetRawBytes<TestContainer6>(blobName);
        rawBytes.ShouldNotBeNull();
        rawBytes!.SequenceEqual(testContent.Reverse().ToArray()).ShouldBeTrue();

        var result = await _container6.GetAllBytesAsync(blobName);
        result.SequenceEqual(testContent).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Keep_Contributor_Scope_Alive_While_Provider_And_Caller_Read_Streams()
    {
        var blobName = "test-blob-scope-bound";
        var testContent = "scope-bound content".GetBytes();

        await _container7.SaveAsync(blobName, testContent);
        using var result = await _container7.GetAsync(blobName);
        using var output = new MemoryStream();
        await result.CopyToAsync(output);

        output.ToArray().ShouldBe(testContent);
    }

    [Fact]
    public async Task Should_Define_Tenant_PassPhrase_As_Encrypted_And_Tenant_Only()
    {
        var definition = await _settingDefinitionManager.GetAsync(BlobStoringEncryptionSettings.TenantPassPhrase);

        definition.IsEncrypted.ShouldBeTrue();
        definition.Providers.ShouldBe(new[] { TenantSettingValueProvider.ProviderName });
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(65536)]
    [InlineData(65537)]
    public void Should_Expose_Exact_Encrypted_Length_For_Seekable_Input(int length)
    {
        using var encryptedStream = _encryptionService.Encrypt(new MemoryStream(new byte[length]), "length-passphrase");
        var reportedLength = encryptedStream.Length;
        using var output = new MemoryStream();

        encryptedStream.CopyTo(output);

        reportedLength.ShouldBe(output.Length);
    }

    [Fact]
    public void Should_Stream_NonSeekable_Unencrypted_Response_Without_Materializing_It()
    {
        var content = new byte[1024 * 1024];
        new Random(42).NextBytes(content);
        var source = new TrackingNonSeekableStream(content);

        using var result = _encryptionService.Decrypt(source, "unused-passphrase");

        source.BytesRead.ShouldBe(5);
        using var output = new MemoryStream();
        result.CopyTo(output);
        output.ToArray().ShouldBe(content);
        result.Dispose();
        source.IsDisposed.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Reject_Encrypted_Blob_Without_Terminal_Record()
    {
        var blobName = "test-blob-truncated-terminal";
        await _container4.SaveAsync(blobName, new byte[128 * 1024]);
        var encryptedBytes = GetRawBytes<TestContainer4>(blobName)!;
        Array.Resize(ref encryptedBytes, encryptedBytes.Length - 20);

        using var decryptedStream = _encryptionService.Decrypt(new MemoryStream(encryptedBytes), "container4-passphrase");
        using var output = new MemoryStream();

        Should.Throw<AbpException>(() => decryptedStream.CopyTo(output));
    }

    [Fact]
    public async Task Should_Reject_Oversized_Encrypted_Blob_Chunk_Size_Before_Allocating()
    {
        var blobName = "test-blob-oversized-chunk";
        await _container4.SaveAsync(blobName, new byte[] { 1 });
        var encryptedBytes = GetRawBytes<TestContainer4>(blobName)!;
        encryptedBytes[7] = 0x7F;
        encryptedBytes[8] = 0xFF;
        encryptedBytes[9] = 0xFF;
        encryptedBytes[10] = 0xFF;

        Should.Throw<AbpException>(() =>
            _encryptionService.Decrypt(new MemoryStream(encryptedBytes), "container4-passphrase")
        );
    }

    private byte[]? GetRawBytes<TContainer>(string blobName)
    {
        return _provider.GetRawBytesOrNull(
            BlobContainerNameAttribute.GetContainerName<TContainer>(),
            blobName
        );
    }

    private byte[] Decrypt(byte[] encryptedBytes, string passPhrase)
    {
        using (var decryptedStream = _encryptionService.Decrypt(new MemoryStream(encryptedBytes), passPhrase))
        using (var memoryStream = new MemoryStream())
        {
            decryptedStream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }

    private sealed class TrackingNonSeekableStream : Stream
    {
        private readonly MemoryStream _stream;

        public int BytesRead { get; private set; }

        public bool IsDisposed { get; private set; }

        public TrackingNonSeekableStream(byte[] bytes)
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
            var readCount = _stream.Read(buffer, offset, count);
            BytesRead += readCount;
            return readCount;
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IsDisposed = true;
                _stream.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
