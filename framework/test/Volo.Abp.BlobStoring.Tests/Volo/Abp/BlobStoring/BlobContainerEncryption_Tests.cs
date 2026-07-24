#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.BlobStoring.Fakes;
using Volo.Abp.BlobStoring.TestObjects;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.BlobStoring;

public class BlobContainerEncryption_Tests : AbpBlobStoringTestBase
{
    private readonly IBlobContainer<TestContainer4> _container4; // UseEncryption("container4-passphrase")
    private readonly IBlobContainer<TestContainer5> _container5; // UseEncryption() -> key provider (tenant setting / global options)
    private readonly IBlobContainer<TestContainer6> _container6; // UseEncryption("container6-passphrase", allowLegacyPlainText: true)
    private readonly FakeInMemoryBlobProvider _provider;
    private readonly ICurrentTenant _currentTenant;

    public BlobContainerEncryption_Tests()
    {
        _container4 = GetRequiredService<IBlobContainer<TestContainer4>>();
        _container5 = GetRequiredService<IBlobContainer<TestContainer5>>();
        _container6 = GetRequiredService<IBlobContainer<TestContainer6>>();
        _provider = GetRequiredService<FakeInMemoryBlobProvider>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
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
    public async Task Should_Save_And_Get_Empty_Blob()
    {
        var blobName = "test-blob-empty";

        await _container4.SaveAsync(blobName, Array.Empty<byte>());

        (await _container4.GetAllBytesAsync(blobName)).ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Record_Key_Source_In_The_Header()
    {
        var blobName = "test-blob-key-source";
        await _container4.SaveAsync(blobName, "content".GetBytes());

        var rawBytes = GetRawBytes<TestContainer4>(blobName)!;

        // magic(4) + version(1) + algorithm(1), then the key source byte
        rawBytes[6].ShouldBe((byte)BlobEncryptionKeySource.Container);
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

            var rawBytes = GetRawBytes<TestContainer5>(blobName);
            rawBytes.ShouldNotBeNull();
            rawBytes![6].ShouldBe((byte)BlobEncryptionKeySource.Tenant);

            (await _container5.GetAllBytesAsync(blobName)).SequenceEqual(testContent).ShouldBeTrue();
        }
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

        var rawBytes1 = GetRawBytes<TestContainer5>("test-blob-tenant-1")!;
        var rawBytes2 = GetRawBytes<TestContainer5>("test-blob-tenant-2")!;
        rawBytes1.SequenceEqual(rawBytes2).ShouldBeFalse();

        // Each tenant can only read its own BLOB
        using (_currentTenant.Change(tenantId1))
        {
            (await _container5.GetAllBytesAsync("test-blob-tenant-1")).SequenceEqual(testContent).ShouldBeTrue();
        }

        using (_currentTenant.Change(tenantId2))
        {
            (await _container5.GetAllBytesAsync("test-blob-tenant-2")).SequenceEqual(testContent).ShouldBeTrue();
        }
    }

    [Fact]
    public async Task Should_Fall_Back_To_Global_PassPhrase_Without_Tenant()
    {
        var blobName = "test-blob-encrypted-global";
        var testContent = "test content".GetBytes();

        await _container5.SaveAsync(blobName, testContent);

        var rawBytes = GetRawBytes<TestContainer5>(blobName);
        rawBytes.ShouldNotBeNull();
        rawBytes![6].ShouldBe((byte)BlobEncryptionKeySource.Global);

        (await _container5.GetAllBytesAsync(blobName)).SequenceEqual(testContent).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Decrypt_With_The_Recorded_Key_Source_Even_If_Other_Keys_Appear_Later()
    {
        var codec = GetRequiredService<BlobEncryptionCodec>();
        var testContent = "written with the global key".GetBytes();

        // Encrypted while only the global passphrase was available
        var globalConfiguration = new BlobContainerConfiguration().UseEncryption();
        using var cipherBytes = new MemoryStream();
        using (var encryptingStream = await codec.CreateEncryptingStreamAsync(globalConfiguration, "routing-container", "routing-blob", null, new MemoryStream(testContent)))
        {
            encryptingStream.CopyTo(cipherBytes);
        }

        // A container passphrase is configured later: the header still routes
        // this BLOB to the global key, so it stays readable.
        var laterConfiguration = new BlobContainerConfiguration().UseEncryption("container-passphrase-added-later");
        using var decryptingStream = await codec.CreateDecryptingStreamAsync(laterConfiguration, "routing-container", "routing-blob", null, new MemoryStream(cipherBytes.ToArray()));
        using var output = new MemoryStream();
        decryptingStream.CopyTo(output);

        output.ToArray().ShouldBe(testContent);
    }

    [Fact]
    public async Task Should_Read_Legacy_Plaintext_Blob_When_Allowed()
    {
        var blobName = "test-blob-legacy";
        var legacyContent = "legacy plain content, stored before encryption was enabled".GetBytes();
        SetRawBytes<TestContainer6>(blobName, legacyContent);

        (await _container6.GetAllBytesAsync(blobName)).SequenceEqual(legacyContent).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Reject_Legacy_Plaintext_Blob_By_Default()
    {
        var blobName = "test-blob-legacy-rejected";
        SetRawBytes<TestContainer4>(blobName, "legacy plain content".GetBytes());

        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            using var stream = await _container4.GetAsync(blobName);
        });
    }

    [Fact]
    public async Task Should_Reject_Encrypted_Blob_With_Tampered_Magic()
    {
        var blobName = "test-blob-tampered-magic";
        await _container4.SaveAsync(blobName, "secret content".GetBytes());

        var rawBytes = GetRawBytes<TestContainer4>(blobName)!;
        rawBytes[0] ^= 0xFF;
        SetRawBytes<TestContainer4>(blobName, rawBytes);

        // Without legacy plaintext enabled this must fail closed instead of
        // returning the raw ciphertext bytes.
        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            using var stream = await _container4.GetAsync(blobName);
        });
    }

    [Fact]
    public async Task Should_Fault_Stream_After_Tampered_Chunk()
    {
        var blobName = "test-blob-tampered-chunk";
        var content = new byte[128 * 1024]; // Spans multiple chunks
        new Random(42).NextBytes(content);
        await _container4.SaveAsync(blobName, content);

        var rawBytes = GetRawBytes<TestContainer4>(blobName)!;
        rawBytes[60] ^= 0xFF; // Inside the first cipher chunk
        SetRawBytes<TestContainer4>(blobName, rawBytes);

        using var stream = await _container4.GetAsync(blobName);
        var buffer = new byte[256 * 1024];

        Assert.ThrowsAny<Exception>(() =>
        {
            while (stream.Read(buffer, 0, buffer.Length) > 0)
            {
            }
        });

        // The stream must stay unreadable; otherwise a caller swallowing the first
        // exception could read the chunks after the tampered one.
        Should.Throw<AbpException>(() => stream.Read(buffer, 0, buffer.Length));
    }

    [Fact]
    public async Task Should_Reject_Encrypted_Blob_Without_Terminal_Record()
    {
        var blobName = "test-blob-truncated-terminal";
        await _container4.SaveAsync(blobName, new byte[128 * 1024]);

        var rawBytes = GetRawBytes<TestContainer4>(blobName)!;
        Array.Resize(ref rawBytes, rawBytes.Length - 20); // terminal record: 4-byte marker + 16-byte tag
        SetRawBytes<TestContainer4>(blobName, rawBytes);

        using var stream = await _container4.GetAsync(blobName);
        using var output = new MemoryStream();

        Should.Throw<AbpException>(() => stream.CopyTo(output));
    }

    [Fact]
    public async Task Should_Reject_Blob_Encrypted_With_Another_PassPhrase()
    {
        var blobName = "test-blob-wrong-key";
        await _container6.SaveAsync(blobName, "content".GetBytes());

        // Same raw bytes, read over a container with a different passphrase
        SetRawBytes<TestContainer4>(blobName, GetRawBytes<TestContainer6>(blobName)!);

        using var stream = await _container4.GetAsync(blobName);
        using var output = new MemoryStream();

        Assert.ThrowsAny<CryptographicException>(() => stream.CopyTo(output));
    }

    [Fact]
    public async Task Should_Save_And_Get_Encrypted_Blob_With_Async_Only_Streams()
    {
        var blobName = "test-blob-async-only";
        var testContent = new byte[192 * 1024]; // Spans multiple encryption chunks
        new Random(42).NextBytes(testContent);

        // Simulates sources like the ASP.NET Core request body, where synchronous reads throw
        await _container4.SaveAsync(blobName, new AsyncOnlyStream(testContent));

        using var result = await _container4.GetAsync(blobName);
        using var output = new MemoryStream();
        await result.CopyToAsync(output);

        output.ToArray().ShouldBe(testContent);
    }

    [Fact]
    public async Task Should_Not_Consume_Data_On_Zero_Byte_Reads()
    {
        var blobName = "test-blob-zero-read";
        var content = new byte[100 * 1024];
        new Random(42).NextBytes(content);
        await _container4.SaveAsync(blobName, content);

        using var stream = await _container4.GetAsync(blobName);
        using var collected = new MemoryStream();
        var buffer = new byte[8 * 1024];

        stream.Read(buffer, 0, 0).ShouldBe(0);
        int readCount;
        while ((readCount = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            collected.Write(buffer, 0, readCount);
            (await stream.ReadAsync(buffer, 0, 0)).ShouldBe(0);
        }

        collected.ToArray().ShouldBe(content);
    }

    [Fact]
    public async Task Should_Not_Allow_Reading_After_Dispose()
    {
        var blobName = "test-blob-dispose";
        await _container4.SaveAsync(blobName, "dispose then read".GetBytes());

        var stream = await _container4.GetAsync(blobName);
        var buffer = new byte[1024];
        stream.Read(buffer, 0, buffer.Length);
        stream.Dispose();

        Should.Throw<ObjectDisposedException>(() => stream.Read(buffer, 0, buffer.Length));
    }

    [Fact]
    public async Task Should_Dispose_Underlying_Stream_Only_Once_On_Mixed_Dispose()
    {
        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("dispose-passphrase");

        using var cipherBytes = new MemoryStream();
        using (var encryptingStream = await codec.CreateEncryptingStreamAsync(configuration, "test-container", "dispose-blob", null, new MemoryStream("dispose once".GetBytes())))
        {
            encryptingStream.CopyTo(cipherBytes);
        }

        var source = new TrackingNonSeekableStream(cipherBytes.ToArray());
        var decryptingStream = await codec.CreateDecryptingStreamAsync(configuration, "test-container", "dispose-blob", null, source);

        await decryptingStream.DisposeAsync();
        decryptingStream.Dispose();

        source.DisposeCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Expose_Exact_Encrypted_Length_For_Seekable_Input()
    {
        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("length-passphrase");

        foreach (var length in new[] { 0, 1, 64 * 1024, 64 * 1024 + 1 })
        {
            using var encryptedStream = await codec.CreateEncryptingStreamAsync(configuration, "test-container", "length-blob", null, new MemoryStream(new byte[length]));
            var reportedLength = encryptedStream.Length;
            using var output = new MemoryStream();

            encryptedStream.CopyTo(output);

            reportedLength.ShouldBe(output.Length);
        }
    }

    [Fact]
    public async Task Should_Expose_Exact_Encrypted_Length_For_A_Length_Aware_Forward_Only_Input()
    {
        // CanSeek is false, but Length/Position are readable; MinIO-like providers need the length
        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("length-passphrase");
        var source = new LengthAwareForwardOnlyStream(new byte[64 * 1024 + 1]);

        using var encryptedStream = await codec.CreateEncryptingStreamAsync(configuration, "test-container", "length-blob-fo", null, source);
        var reportedLength = encryptedStream.Length;
        using var output = new MemoryStream();

        encryptedStream.CopyTo(output);

        reportedLength.ShouldBe(output.Length);
    }

    private sealed class LengthAwareForwardOnlyStream : Stream
    {
        private readonly MemoryStream _stream;

        public LengthAwareForwardOnlyStream(byte[] bytes)
        {
            _stream = new MemoryStream(bytes);
        }

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => _stream.Length;

        public override long Position
        {
            get => _stream.Position;
            set => throw new NotSupportedException();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, count);
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

    [Fact]
    public async Task Should_Reject_A_Blob_Copied_To_Another_Name()
    {
        await _container4.SaveAsync("identity-a", "content of a".GetBytes());
        await _container4.SaveAsync("identity-b", "content of b".GetBytes());

        // Same container, same passphrase: only the AAD identity binding can catch this
        SetRawBytes<TestContainer4>("identity-a", GetRawBytes<TestContainer4>("identity-b")!);

        using var stream = await _container4.GetAsync("identity-a");
        using var output = new MemoryStream();

        Assert.ThrowsAny<CryptographicException>(() => stream.CopyTo(output));
    }

    [Fact]
    public async Task Should_Reject_A_Blob_Served_From_Another_Tenants_Location()
    {
        var blobName = "identity-tenant";
        using (_currentTenant.Change(Guid.NewGuid()))
        {
            await _container4.SaveAsync(blobName, "tenant content".GetBytes());
        }

        // The fake provider stores host and tenant blobs in the same slot, so this
        // simulates moving the encrypted bytes to the host location. Same passphrase,
        // different identity: must fail.
        using var stream = await _container4.GetAsync(blobName);
        using var output = new MemoryStream();

        Assert.ThrowsAny<CryptographicException>(() => stream.CopyTo(output));
    }

    [Fact]
    public async Task Should_Share_Blobs_Of_A_Non_MultiTenant_Container_Between_Tenants()
    {
        var container7 = GetRequiredService<IBlobContainer<TestContainer7>>(); // IsMultiTenant = false
        var blobName = "shared-container-blob";
        var testContent = "shared content".GetBytes();

        using (_currentTenant.Change(Guid.NewGuid()))
        {
            await container7.SaveAsync(blobName, testContent);
        }

        // A shared container always uses the host identity, so every tenant
        // (and the host) reads the same BLOB.
        using (_currentTenant.Change(Guid.NewGuid()))
        {
            (await container7.GetAllBytesAsync(blobName)).SequenceEqual(testContent).ShouldBeTrue();
        }

        (await container7.GetAllBytesAsync(blobName)).SequenceEqual(testContent).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Throw_When_Cancelled_Before_Saving()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
            await _container4.SaveAsync("cancelled-save", new MemoryStream("content".GetBytes()), cancellationToken: cts.Token);
        });
    }

    [Fact]
    public async Task Should_Throw_When_Cancelled_Before_Reading()
    {
        await _container4.SaveAsync("cancelled-read", "content".GetBytes());

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
            using var stream = await _container4.GetAsync("cancelled-read", cts.Token);
        });
    }

    [Fact]
    public async Task Should_Reject_Tenant_Key_Source_With_The_Default_Key_Provider()
    {
        var defaultProvider = new DefaultBlobEncryptionKeyProvider(
            Microsoft.Extensions.Options.Options.Create(new AbpBlobStoringEncryptionOptions())
        );

        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await defaultProvider.ResolveForDecryptionAsync(
                BlobEncryptionKeySource.Tenant,
                new BlobEncryptionKeyContext(new BlobContainerConfiguration(), "c", "b", null));
        });
    }

    [Fact]
    public async Task Should_Throw_When_No_PassPhrase_Can_Be_Resolved()
    {
        var defaultProvider = new DefaultBlobEncryptionKeyProvider(
            Microsoft.Extensions.Options.Options.Create(new AbpBlobStoringEncryptionOptions())
        );
        var configuration = new BlobContainerConfiguration().UseEncryption();

        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await defaultProvider.ResolveForEncryptionAsync(
                new BlobEncryptionKeyContext(configuration, "c", "b", null));
        });
    }

    [Fact]
    public async Task Should_Return_Raw_Bytes_When_Encryption_Is_Not_Enabled_For_The_Container()
    {
        // Simulates reading an encrypted BLOB over a container without encryption
        // (like after DisableEncryption): the stored bytes are returned as-is.
        var blobName = "raw-ciphertext-readback";
        await _container4.SaveAsync(blobName, "content".GetBytes());
        var encryptedBytes = GetRawBytes<TestContainer4>(blobName)!;

        var container8 = GetRequiredService<IBlobContainer<TestContainer8>>(); // no encryption
        _provider.SetRawBytes(
            BlobContainerNameAttribute.GetContainerName<TestContainer8>(),
            blobName,
            encryptedBytes
        );

        (await container8.GetAllBytesAsync(blobName)).SequenceEqual(encryptedBytes).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Reject_An_Unknown_Format_Version()
    {
        var blobName = "unknown-format-version";
        await _container4.SaveAsync(blobName, "content".GetBytes());

        var rawBytes = GetRawBytes<TestContainer4>(blobName)!;
        rawBytes[4] = 2; // Format version byte
        SetRawBytes<TestContainer4>(blobName, rawBytes);

        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            using var stream = await _container4.GetAsync(blobName);
        });
    }

    [Fact]
    public async Task Should_Read_A_Legacy_Blob_Shorter_Than_The_Format_Magic()
    {
        var blobName = "tiny-legacy-blob";
        var tinyContent = new byte[] { 1, 2, 3 };
        SetRawBytes<TestContainer6>(blobName, tinyContent); // allowLegacyPlainText: true

        (await _container6.GetAllBytesAsync(blobName)).SequenceEqual(tinyContent).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Use_The_Configured_Kdf_Iterations()
    {
        var options = Microsoft.Extensions.Options.Options.Create(new AbpBlobStoringEncryptionOptions
        {
            DefaultPassPhrase = "iterations-passphrase",
            KdfIterations = 150_000
        });
        var codec = new BlobEncryptionCodec(new DefaultBlobEncryptionKeyProvider(options), options);
        var configuration = new BlobContainerConfiguration().UseEncryption();

        using var cipherBytes = new MemoryStream();
        using (var encryptingStream = await codec.CreateEncryptingStreamAsync(configuration, "iter-container", "iter-blob", null, new MemoryStream("content".GetBytes())))
        {
            encryptingStream.CopyTo(cipherBytes);
        }

        var rawBytes = cipherBytes.ToArray();
        var recordedIterations = (rawBytes[7] << 24) | (rawBytes[8] << 16) | (rawBytes[9] << 8) | rawBytes[10];
        recordedIterations.ShouldBe(150_000);

        using var decryptingStream = await codec.CreateDecryptingStreamAsync(configuration, "iter-container", "iter-blob", null, new MemoryStream(rawBytes));
        using var output = new MemoryStream();
        decryptingStream.CopyTo(output);
        output.ToArray().ShouldBe("content".GetBytes());
    }

    [Fact]
    public async Task Should_Reject_Kdf_Iterations_Out_Of_The_Allowed_Range()
    {
        foreach (var iterations in new[] { 50_000, 700_000 })
        {
            var options = Microsoft.Extensions.Options.Options.Create(new AbpBlobStoringEncryptionOptions
            {
                DefaultPassPhrase = "iterations-passphrase",
                KdfIterations = iterations
            });
            var codec = new BlobEncryptionCodec(new DefaultBlobEncryptionKeyProvider(options), options);
            var configuration = new BlobContainerConfiguration().UseEncryption();

            await Assert.ThrowsAsync<AbpException>(async () =>
            {
                await codec.CreateEncryptingStreamAsync(configuration, "iter-container", "iter-blob", null, new MemoryStream("content".GetBytes()));
            });
        }
    }

    [Fact]
    public void Should_Reject_Wrapped_Chunk_Index()
    {
        // The production streams write the chunk index into a reusable nonce/AAD buffer with
        // WriteChunkIndex, so a wrapped (negative) index must be rejected there
        Should.Throw<AbpException>(() => BlobEncryptionCodec.WriteChunkIndex(new byte[BlobEncryptionCodec.GcmNonceSize], -1));
    }

    [Fact]
    public void Should_Reject_Unknown_Key_Source()
    {
        // An unknown source would be written into the header and make the BLOB unreadable
        Assert.ThrowsAny<ArgumentException>(() => new BlobEncryptionKey((BlobEncryptionKeySource)99, "passphrase"));
        Assert.ThrowsAny<ArgumentException>(() => new BlobEncryptionKey(0, "passphrase"));
    }

    [Fact]
    public async Task Should_Reject_Tampered_Kdf_Iterations()
    {
        var blobName = "test-blob-tampered-iterations";
        await _container4.SaveAsync(blobName, "content".GetBytes());

        var rawBytes = GetRawBytes<TestContainer4>(blobName)!;
        rawBytes[7] = 0x7F; // Iterations field: far above the allowed maximum
        SetRawBytes<TestContainer4>(blobName, rawBytes);

        // Must be rejected before deriving the key (a huge iteration count would be a CPU DoS)
        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            using var stream = await _container4.GetAsync(blobName);
        });
    }

    [Fact]
    public async Task Should_Reject_Tampered_Header_Salt()
    {
        var blobName = "test-blob-tampered-salt";
        await _container4.SaveAsync(blobName, "content".GetBytes());

        var rawBytes = GetRawBytes<TestContainer4>(blobName)!;
        rawBytes[12] ^= 0xFF; // Inside the KDF salt; the header is bound to every chunk as AAD
        SetRawBytes<TestContainer4>(blobName, rawBytes);

        using var stream = await _container4.GetAsync(blobName);
        using var output = new MemoryStream();

        Assert.ThrowsAny<CryptographicException>(() => stream.CopyTo(output));
    }

    [Fact]
    public async Task Should_Not_Decrypt_Another_Tenants_Blob()
    {
        var blobName = "test-blob-cross-tenant";
        var testContent = "tenant 1 secret".GetBytes();

        using (_currentTenant.Change(Guid.NewGuid()))
        {
            await _container5.SaveAsync(blobName, testContent);
        }

        // Another tenant resolves its own passphrase for the same key source
        using (_currentTenant.Change(Guid.NewGuid()))
        {
            using var stream = await _container5.GetAsync(blobName);
            using var output = new MemoryStream();

            Assert.ThrowsAny<CryptographicException>(() => stream.CopyTo(output));
        }
    }

    [Fact]
    public async Task Should_Keep_The_V1_Format_Stable()
    {
        // Golden vector: deterministic ciphertext built from fixed inputs.
        // If this test breaks, the on-disk format changed and existing
        // encrypted BLOBs would become unreadable.
        const string expected =
            "QUJQRQEBAQABhqABAgMEBQYHCAkKCwwNDg8QAAEAAKChoqOkpaanAAAAFaa92MyQIqeyxK979tS+roEOv5MJTSc3BsF2sVmrewTHYbj3/UsAAAAAgMPye1nJF2Vd05yIpGw5Kg==";

        var salt = new byte[16];
        var baseNonce = new byte[8];
        for (var i = 0; i < 16; i++) salt[i] = (byte)(i + 1);
        for (var i = 0; i < 8; i++) baseNonce[i] = (byte)(0xA0 + i);

        var keyBytes = BlobEncryptionCodec.DeriveKeyBytes("golden-passphrase", salt, 100_000);
        var header = BlobEncryptionCodec.BuildHeader(BlobEncryptionKeySource.Container, 100_000, salt, 64 * 1024, baseNonce);
        var prefix = BlobEncryptionCodec.CreateBlobPrefix(header);
        var aad = BlobEncryptionCodec.BuildAssociatedDataPrefix(prefix, "golden-container", "golden-blob", null);
        var plain = "golden vector content"u8.ToArray();

        using var cipher = new MemoryStream();
        cipher.Write(prefix, 0, prefix.Length);
        var chunkRecord = BlobEncryptionCodec.EncryptChunk(keyBytes, aad, baseNonce, 0, plain, plain.Length);
        cipher.Write(chunkRecord, 0, chunkRecord.Length);
        var terminalRecord = BlobEncryptionCodec.CreateTerminalRecord(keyBytes, aad, baseNonce, 1);
        cipher.Write(terminalRecord, 0, terminalRecord.Length);

        Convert.ToBase64String(cipher.ToArray()).ShouldBe(expected);

        // And the golden ciphertext must keep decrypting to the original content
        cipher.Position = prefix.Length;
        using var decryptingStream = new ChunkedDecryptingReadStream(cipher, aad, keyBytes, baseNonce, 64 * 1024);
        using var output = new MemoryStream();
        decryptingStream.CopyTo(output);
        output.ToArray().ShouldBe(plain);

        // The full production reader must also keep reading historical v1 data:
        // header parsing, key routing and the decrypting state machine included
        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("golden-passphrase");
        using var historicalBlob = new MemoryStream(Convert.FromBase64String(expected));
        using var readerStream = await codec.CreateDecryptingStreamAsync(configuration, "golden-container", "golden-blob", null, historicalBlob);
        using var readerOutput = new MemoryStream();
        await readerStream.CopyToAsync(readerOutput);
        readerOutput.ToArray().ShouldBe(plain);
    }

    [Fact]
    public async Task Should_Keep_The_V1_Format_Stable_For_Tenant_Blobs()
    {
        // Pins the AAD encoding of the tenant id: BLOBs of a tenant, stored with
        // the v1 format, must stay readable by the current production reader
        const string expected =
            "QUJQRQEBAQABhqABAgMEBQYHCAkKCwwNDg8QAAEAAKChoqOkpaanAAAAHKa92MyQIqewxKJu99K+u4sDv5kVGZfI/vGgGbUD0JfeWzh4JbOYAQz5Axr1AAAAAIAHZhGwF81jmA7v8d0GZF0=";

        var salt = new byte[16];
        var baseNonce = new byte[8];
        for (var i = 0; i < 16; i++) salt[i] = (byte)(i + 1);
        for (var i = 0; i < 8; i++) baseNonce[i] = (byte)(0xA0 + i);
        var tenantId = new Guid("11111111-2222-3333-4444-555555555555");

        var keyBytes = BlobEncryptionCodec.DeriveKeyBytes("golden-passphrase", salt, 100_000);
        var header = BlobEncryptionCodec.BuildHeader(BlobEncryptionKeySource.Container, 100_000, salt, 64 * 1024, baseNonce);
        var prefix = BlobEncryptionCodec.CreateBlobPrefix(header);
        var aad = BlobEncryptionCodec.BuildAssociatedDataPrefix(prefix, "golden-container", "golden-blob", tenantId);
        var plain = "golden tenant vector content"u8.ToArray();

        using var cipher = new MemoryStream();
        cipher.Write(prefix, 0, prefix.Length);
        var chunkRecord = BlobEncryptionCodec.EncryptChunk(keyBytes, aad, baseNonce, 0, plain, plain.Length);
        cipher.Write(chunkRecord, 0, chunkRecord.Length);
        var terminalRecord = BlobEncryptionCodec.CreateTerminalRecord(keyBytes, aad, baseNonce, 1);
        cipher.Write(terminalRecord, 0, terminalRecord.Length);

        Convert.ToBase64String(cipher.ToArray()).ShouldBe(expected);

        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("golden-passphrase");
        using var historicalBlob = new MemoryStream(Convert.FromBase64String(expected));
        using var readerStream = await codec.CreateDecryptingStreamAsync(configuration, "golden-container", "golden-blob", tenantId, historicalBlob);
        using var output = new MemoryStream();
        await readerStream.CopyToAsync(output);
        output.ToArray().ShouldBe(plain);
    }

    [Fact]
    public async Task Should_Encrypt_And_Decrypt_A_Modern_Async_Only_Source()
    {
        var content = new byte[100_000];
        new Random(42).NextBytes(content);

        using (var source = new FakeModernAsyncOnlyStream(new MemoryStream(content)))
        {
            await _container4.SaveAsync("modern-source", source, overrideExisting: true);
        }

        (await _container4.GetAllBytesAsync("modern-source")).SequenceEqual(content).ShouldBeTrue();

        // A modern-async-only cipher stream (like a modern provider response) works too
        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("modern-passphrase");
        byte[] cipherBytes;
        using (var encryptingStream = await codec.CreateEncryptingStreamAsync(configuration, "modern-container", "modern-blob", null, new MemoryStream(content)))
        using (var cipherBuffer = new MemoryStream())
        {
            await encryptingStream.CopyToAsync(cipherBuffer);
            cipherBytes = cipherBuffer.ToArray();
        }

        using var decryptingStream = await codec.CreateDecryptingStreamAsync(
            configuration, "modern-container", "modern-blob", null, new FakeModernAsyncOnlyStream(new MemoryStream(cipherBytes)));
        using var output = new MemoryStream();
        await decryptingStream.CopyToAsync(output);
        output.ToArray().SequenceEqual(content).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Reject_Content_Truncated_Right_After_The_Magic_Even_When_Legacy_Is_Allowed()
    {
        // A full "ABPE" magic already identifies the encrypted format: it must fail
        // as corrupted instead of being returned as (unauthenticated) legacy plaintext
        SetRawBytes<TestContainer6>("magic-only", "ABPE"u8.ToArray());

        var exception = await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _container6.GetAsync("magic-only");
        });

        exception.Message.ShouldContain("missing format version");
    }

    [Fact]
    public async Task Should_Reject_A_Header_With_Fewer_Iterations_Than_The_Writer_Minimum()
    {
        // Accepting fewer iterations than any legitimate writer ever used would let
        // attacker-crafted content turn reads into a cheap passphrase-guessing oracle
        var salt = new byte[16];
        var baseNonce = new byte[8];
        const int lowIterations = 50_000;

        var keyBytes = BlobEncryptionCodec.DeriveKeyBytes("oracle-passphrase", salt, lowIterations);
        var header = BlobEncryptionCodec.BuildHeader(BlobEncryptionKeySource.Container, lowIterations, salt, 64 * 1024, baseNonce);
        var prefix = BlobEncryptionCodec.CreateBlobPrefix(header);
        var aad = BlobEncryptionCodec.BuildAssociatedDataPrefix(prefix, "oracle-container", "oracle-blob", null);
        var plain = "oracle content"u8.ToArray();

        using var cipher = new MemoryStream();
        cipher.Write(prefix, 0, prefix.Length);
        var chunkRecord = BlobEncryptionCodec.EncryptChunk(keyBytes, aad, baseNonce, 0, plain, plain.Length);
        cipher.Write(chunkRecord, 0, chunkRecord.Length);
        cipher.Position = 0;

        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("oracle-passphrase");

        var exception = await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await codec.CreateDecryptingStreamAsync(configuration, "oracle-container", "oracle-blob", null, cipher);
        });

        exception.Message.ShouldContain("invalid KDF iteration count");
    }

    [Fact]
    public async Task Should_Not_Expose_A_Length_When_The_Position_Is_Not_Readable()
    {
        // Length is known but Position throws: the remaining length is genuinely
        // unknown (the stream may be partially consumed), so no length is exposed
        // rather than a guessed (possibly too-long) one that would short-write
        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("length-passphrase");
        var content = new byte[64 * 1024 + 1];
        using var source = new LengthOnlyStream(content);

        using var encryptedStream = await codec.CreateEncryptingStreamAsync(configuration, "length-container", "length-blob", null, source);

        Should.Throw<NotSupportedException>(() => encryptedStream.Length);

        // The content still round-trips correctly, only the length is unknown
        using var output = new MemoryStream();
        await encryptedStream.CopyToAsync(output);
        output.Length.ShouldBeGreaterThan(content.Length);
    }

    private sealed class LengthOnlyStream : Stream
    {
        private readonly MemoryStream _inner;

        public LengthOnlyStream(byte[] bytes)
        {
            _inner = new MemoryStream(bytes);
        }

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => _inner.Length;

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
            if (disposing)
            {
                _inner.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    [Fact]
    public async Task Should_Save_A_Source_Whose_Length_Probe_Fails()
    {
        var content = "length probe failure content".GetBytes();
        using var source = new FakeIoFailingLengthStream(new MemoryStream(content));

        await _container4.SaveAsync("length-probe-failure", source, overrideExisting: true);

        (await _container4.GetAllBytesAsync("length-probe-failure")).ShouldBe(content);
    }

    [Fact]
    public async Task Should_Keep_The_V1_Writer_Output_Stable_For_Multi_Chunk_Content()
    {
        // Pins the production writer state machine (prefix, chunk framing, terminal
        // record) on multi-chunk content, and that the production reader reads it
        const string expected =
            "QUJQRQEBAQABhqABAgMEBQYHCAkKCwwNDg8QAAAAEKChoqOkpaanAAAAEKa92MyQIqep1KB78Ib9pZvT8su7m9hTNktwSiqr1SWvAAAAEEnATYRJsL9GiPFOI/UsQ+rri8bl03Het8U62zMtQuKGAAAACALRHX4ndOvYmGqf4ahSHre7Jn85GCHKZAAAAAATN3tj7GdkC3MxveC+K07f";

        var salt = new byte[16];
        var baseNonce = new byte[8];
        for (var i = 0; i < 16; i++) salt[i] = (byte)(i + 1);
        for (var i = 0; i < 8; i++) baseNonce[i] = (byte)(0xA0 + i);

        var keyBytes = BlobEncryptionCodec.DeriveKeyBytes("golden-passphrase", salt, 100_000);
        var header = BlobEncryptionCodec.BuildHeader(BlobEncryptionKeySource.Container, 100_000, salt, 16, baseNonce);
        var prefix = BlobEncryptionCodec.CreateBlobPrefix(header);
        var aad = BlobEncryptionCodec.BuildAssociatedDataPrefix(prefix, "golden-container", "golden-blob", null);
        var plain = "golden multi chunk writer vector content"u8.ToArray(); // 40 bytes -> 3 chunks of 16

        using var writerStream = new ChunkedEncryptingReadStream(new MemoryStream(plain), prefix, aad, (byte[])keyBytes.Clone(), baseNonce, 16, null);
        using var cipher = new MemoryStream();
        await writerStream.CopyToAsync(cipher);

        Convert.ToBase64String(cipher.ToArray()).ShouldBe(expected);

        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("golden-passphrase");
        using var historicalBlob = new MemoryStream(Convert.FromBase64String(expected));
        using var readerStream = await codec.CreateDecryptingStreamAsync(configuration, "golden-container", "golden-blob", null, historicalBlob);
        using var output = new MemoryStream();
        await readerStream.CopyToAsync(output);
        output.ToArray().ShouldBe(plain);
    }

    [Fact]
    public async Task Should_Keep_The_Exact_Length_When_Re_Encrypting_A_Legacy_Stream()
    {
        var content = new byte[1000];
        new Random(42).NextBytes(content);

        // Simulate the legacy replay stream: the first bytes were consumed as the format probe
        var underlying = new MemoryStream(content);
        var probeBytes = new byte[5];
        underlying.Read(probeBytes, 0, probeBytes.Length);
        using var legacyStream = new PrefixingReadStream(probeBytes, underlying);

        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("legacy-reencrypt-passphrase");
        using var encryptingStream = await codec.CreateEncryptingStreamAsync(configuration, "legacy-container", "legacy-blob", null, legacyStream);

        var reportedLength = encryptingStream.Length; // Length - Position of the legacy stream is known
        using var output = new MemoryStream();
        await encryptingStream.CopyToAsync(output);

        reportedLength.ShouldBe(output.Length);
    }

    [Fact]
    public async Task Should_Reject_Content_Too_Large_For_The_Chunk_Index_Upfront()
    {
        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("huge-passphrase");

        var exception = await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await codec.CreateEncryptingStreamAsync(configuration, "huge-container", "huge-blob", null, new HugeLengthStream());
        });

        exception.Message.ShouldContain("too large");
    }

    private sealed class HugeLengthStream : Stream
    {
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => 200_000_000_000_000_000; // Far beyond int.MaxValue chunks

        public override long Position
        {
            get => 0;
            set => throw new NotSupportedException();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count) => 0;
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }

    [Fact]
    public async Task Should_Reject_A_Blob_Name_With_Invalid_Utf16()
    {
        // Different unpaired surrogates would fold into the same replacement bytes,
        // giving two different names the same authenticated identity
        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("surrogate-passphrase");

        var exception = await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await codec.CreateEncryptingStreamAsync(configuration, "surrogate-container", "x\uD800", null, new MemoryStream());
        });

        exception.Message.ShouldContain("invalid characters");
    }

    [Fact]
    public void Should_Report_The_Served_Length_Of_A_Legacy_Prefixing_Stream_When_The_Underlying_Is_Offset()
    {
        // The underlying provider stream is not at position 0: the prefixing wrapper
        // must report prefix + remaining, not the underlying total length
        var content = new byte[1000];
        new Random(42).NextBytes(content);
        var underlying = new MemoryStream(content);
        underlying.Position = 2; // Simulate a provider stream that did not start at 0

        var probe = new byte[5];
        underlying.Read(probe, 0, probe.Length); // The magic probe consumed 5 more bytes

        using var prefixing = new PrefixingReadStream(probe, underlying);

        // Served = prefix (5) + remaining underlying (1000 - 7) = 998
        prefixing.Length.ShouldBe(content.Length - 2);

        using var output = new MemoryStream();
        prefixing.CopyTo(output);
        output.Length.ShouldBe(content.Length - 2);
    }

    [Fact]
    public void Should_Report_The_Bytes_Served_As_The_Position_Of_A_Prefixing_Stream()
    {
        var underlying = new MemoryStream(new byte[100]);
        using var prefixing = new PrefixingReadStream(new byte[5], underlying);

        var buffer = new byte[105];
        var read = prefixing.Read(buffer, 0, buffer.Length);

        (prefixing.Length - prefixing.Position).ShouldBe(prefixing.Length - read);
    }

    [Fact]
    public async Task Should_Pass_The_Blob_Identity_To_The_Key_Provider()
    {
        // The key context carries the normalized container/BLOB name and the tenant,
        // so a custom provider can select the key by the BLOB identity
        // The codec is a public, replaceable service; construct it with a recording key
        // provider directly instead of reaching into its internals
        var recordingProvider = new RecordingKeyProvider();
        var codec = new BlobEncryptionCodec(
            recordingProvider,
            GetRequiredService<IOptions<AbpBlobStoringEncryptionOptions>>());

        var tenantId = Guid.NewGuid();
        var configuration = new BlobContainerConfiguration().UseEncryption("identity-passphrase");
        using var stream = await codec.CreateEncryptingStreamAsync(configuration, "the-container", "the-blob", tenantId, new MemoryStream());
        using var output = new MemoryStream();
        await stream.CopyToAsync(output);

        recordingProvider.LastContext.ShouldNotBeNull();
        recordingProvider.LastContext!.ContainerName.ShouldBe("the-container");
        recordingProvider.LastContext.BlobName.ShouldBe("the-blob");
        recordingProvider.LastContext.TenantId.ShouldBe(tenantId);
    }

    private sealed class RecordingKeyProvider : IBlobEncryptionKeyProvider
    {
        public BlobEncryptionKeyContext? LastContext { get; private set; }

        public Task<BlobEncryptionKey> ResolveForEncryptionAsync(BlobEncryptionKeyContext context, CancellationToken cancellationToken = default)
        {
            LastContext = context;
            return Task.FromResult(new BlobEncryptionKey(BlobEncryptionKeySource.Container, "identity-passphrase"));
        }

        public Task<string> ResolveForDecryptionAsync(BlobEncryptionKeySource keySource, BlobEncryptionKeyContext context, CancellationToken cancellationToken = default)
        {
            LastContext = context;
            return Task.FromResult("identity-passphrase");
        }
    }

    [Fact]
    public async Task Should_Not_Recover_A_Faulted_Decrypting_Stream_In_The_End_Check()
    {
        // Insert a forged chunk record (no key needed) before the terminal record. Reading
        // it faults the stream without incrementing the chunk index, so the original
        // terminal would still verify at the same index — the end check must keep the fault
        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("fault-passphrase");

        byte[] cipher;
        using (var encrypting = await codec.CreateEncryptingStreamAsync(configuration, "fault-container", "fault-blob", null, new MemoryStream()))
        using (var buffer = new MemoryStream())
        {
            await encrypting.CopyToAsync(buffer);
            cipher = buffer.ToArray();
        }

        // A forged content record: 4-byte length + 16 random cipher bytes + 16 random tag
        var forged = new byte[4 + 16 + 16];
        forged[0] = 0; forged[1] = 0; forged[2] = 0; forged[3] = 16;
        new Random(7).NextBytes(forged.AsSpan(4));

        // Splice it in right before the 20-byte terminal record
        var tampered = new byte[cipher.Length + forged.Length];
        Array.Copy(cipher, 0, tampered, 0, cipher.Length - 20);
        Array.Copy(forged, 0, tampered, cipher.Length - 20, forged.Length);
        Array.Copy(cipher, cipher.Length - 20, tampered, cipher.Length - 20 + forged.Length, 20);

        using var decrypting = await codec.CreateDecryptingStreamAsync(configuration, "fault-container", "fault-blob", null, new MemoryStream(tampered));
        var readBuffer = new byte[1024];

        // Reading the forged record faults the stream
        Assert.ThrowsAny<Exception>(() =>
        {
            while (decrypting.Read(readBuffer, 0, readBuffer.Length) > 0)
            {
            }
        });

        // The end check must not "recover" the faulted stream by verifying the terminal
        Should.Throw<Exception>(() => ((IBlobAuthenticatedEndStream)decrypting).EnsureReadToAuthenticatedEnd());
    }

    [Fact]
    public async Task Should_Reject_Data_Appended_After_The_Terminal_Record()
    {
        // The terminal record marks the authenticated end; any trailing bytes after it mean
        // the stored ciphertext was extended, so reading to the end must fail
        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("append-passphrase");
        var content = "content that ends cleanly".GetBytes();

        byte[] cipher;
        using (var encrypting = await codec.CreateEncryptingStreamAsync(configuration, "append-container", "append-blob", null, new MemoryStream(content)))
        using (var buffer = new MemoryStream())
        {
            await encrypting.CopyToAsync(buffer);
            cipher = buffer.ToArray();
        }

        // Append one byte after the valid terminal record
        var appended = new byte[cipher.Length + 1];
        Array.Copy(cipher, appended, cipher.Length);
        appended[cipher.Length] = 0x42;

        using var decrypting = await codec.CreateDecryptingStreamAsync(configuration, "append-container", "append-blob", null, new MemoryStream(appended));
        var readBuffer = new byte[content.Length + 1024];

        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            while (await decrypting.ReadAsync(readBuffer, 0, readBuffer.Length) > 0)
            {
            }
        });
    }

    [Fact]
    public async Task Should_Not_Fault_A_Healthy_Stream_When_The_End_Check_Is_Cancelled()
    {
        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("cancel-passphrase");
        var content = "cancel end-check content".GetBytes();

        byte[] cipher;
        using (var encrypting = await codec.CreateEncryptingStreamAsync(configuration, "cancel-container", "cancel-blob", null, new MemoryStream(content)))
        using (var buffer = new MemoryStream())
        {
            await encrypting.CopyToAsync(buffer);
            cipher = buffer.ToArray();
        }

        var cancelSource = new CancellationHonoringStream(new MemoryStream(cipher));
        using var decryptingStream = await codec.CreateDecryptingStreamAsync(configuration, "cancel-container", "cancel-blob", null, cancelSource);
        var decrypting = (IBlobAuthenticatedEndStream)decryptingStream;

        // Consume all content, but not the terminal record yet
        var readBuffer = new byte[content.Length];
        var total = 0;
        while (total < readBuffer.Length)
        {
            var read = await decryptingStream.ReadAsync(readBuffer.AsMemory(total, readBuffer.Length - total));
            total += read;
        }
        readBuffer.ShouldBe(content);

        // The terminal read is cancelled: it must not permanently fault the healthy stream
        cancelSource.HonorCancellation = true;
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
            await decrypting.EnsureReadToAuthenticatedEndAsync(new CancellationToken(canceled: true));
        });

        // A retry with a live token still verifies the terminal record
        cancelSource.HonorCancellation = false;
        await decrypting.EnsureReadToAuthenticatedEndAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Should_Fault_When_The_End_Check_Is_Cancelled_After_Consuming_Part_Of_The_Terminal_Record()
    {
        // A cancellation before any I/O leaves the stream healthy (see the test above), but a
        // cancellation after the terminal record was partially consumed can not: the consumed
        // bytes are gone from the non-seekable cipher stream, so a retry would parse from the
        // middle of the record and misreport a valid BLOB as corrupt. The stream must fault
        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("partial-cancel-passphrase");
        var content = "partial cancel end-check content".GetBytes();

        byte[] cipher;
        using (var encrypting = await codec.CreateEncryptingStreamAsync(configuration, "partial-cancel-container", "partial-cancel-blob", null, new MemoryStream(content)))
        using (var buffer = new MemoryStream())
        {
            await encrypting.CopyToAsync(buffer);
            cipher = buffer.ToArray();
        }

        var cancelSource = new PartialReadThenCancelStream(new MemoryStream(cipher));
        using var decryptingStream = await codec.CreateDecryptingStreamAsync(configuration, "partial-cancel-container", "partial-cancel-blob", null, cancelSource);
        var decrypting = (IBlobAuthenticatedEndStream)decryptingStream;

        // Consume all content, but not the terminal record yet
        var readBuffer = new byte[content.Length];
        var total = 0;
        while (total < readBuffer.Length)
        {
            var read = await decryptingStream.ReadAsync(readBuffer.AsMemory(total, readBuffer.Length - total));
            total += read;
        }
        readBuffer.ShouldBe(content);

        // The terminal read consumes one byte and is then cancelled mid-record
        cancelSource.TripOnNextReads = true;
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
            await decrypting.EnsureReadToAuthenticatedEndAsync(CancellationToken.None);
        });

        // The stream must now be faulted: a retry must report the fault, not resume parsing
        // from the middle of the terminal record and surface a false corruption error
        cancelSource.TripOnNextReads = false;
        var retry = await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await decrypting.EnsureReadToAuthenticatedEndAsync(CancellationToken.None);
        });
        retry.Message.ShouldContain("a previous read operation has failed");
    }

    private sealed class PartialReadThenCancelStream : Stream
    {
        private readonly Stream _inner;
        private int _tripStep;

        // When set, the next read returns a single byte and the read after it throws
        // OperationCanceledException, simulating a provider that consumes part of the
        // terminal record and is then cancelled mid-read
        public bool TripOnNextReads { get; set; }

        public PartialReadThenCancelStream(Stream inner)
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

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return ReadTrippedAsync(buffer.AsMemory(offset, count), cancellationToken).AsTask();
        }

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            return ReadTrippedAsync(buffer, cancellationToken);
        }

        private ValueTask<int> ReadTrippedAsync(Memory<byte> buffer, CancellationToken cancellationToken)
        {
            if (TripOnNextReads)
            {
                if (_tripStep == 0)
                {
                    _tripStep++;
                    // Consume a single byte of the terminal record before the cancellation
                    return _inner.ReadAsync(buffer.Slice(0, Math.Min(1, buffer.Length)), cancellationToken);
                }

                throw new OperationCanceledException();
            }

            return _inner.ReadAsync(buffer, cancellationToken);
        }

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

    private sealed class CancellationHonoringStream : Stream
    {
        private readonly Stream _inner;

        public bool HonorCancellation { get; set; }

        public CancellationHonoringStream(Stream inner)
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

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (HonorCancellation && cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException(cancellationToken);
            }

            return _inner.ReadAsync(buffer, cancellationToken);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (HonorCancellation && cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException(cancellationToken);
            }

            return _inner.ReadAsync(buffer, offset, count, cancellationToken);
        }

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

    [Fact]
    public async Task Should_Not_Expose_A_Legacy_Plaintext_Stream_As_Authenticated_End()
    {
        // Legacy plaintext has no authenticated terminal, so its stream must not claim to
        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("legacy-passphrase", allowLegacyPlainText: true);

        using var legacy = await codec.CreateDecryptingStreamAsync(configuration, "legacy-container", "legacy-blob", null, new MemoryStream("plain content".GetBytes()));

        legacy.ShouldNotBeAssignableTo<IBlobAuthenticatedEndStream>();
    }

    [Fact]
    public async Task Should_Reject_A_PassPhrase_With_Invalid_Utf16()
    {
        // Consistent across target frameworks: an unpaired surrogate passphrase is rejected
        var codec = GetRequiredService<BlobEncryptionCodec>();
        var configuration = new BlobContainerConfiguration().UseEncryption("x\uD800");

        var exception = await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await codec.CreateEncryptingStreamAsync(configuration, "surrogate-pass-container", "b", null, new MemoryStream());
        });

        exception.Message.ShouldContain("invalid characters");
    }

    private byte[]? GetRawBytes<TContainer>(string blobName)
    {
        return _provider.GetRawBytesOrNull(
            BlobContainerNameAttribute.GetContainerName<TContainer>(),
            blobName
        );
    }

    private void SetRawBytes<TContainer>(string blobName, byte[] bytes)
    {
        _provider.SetRawBytes(
            BlobContainerNameAttribute.GetContainerName<TContainer>(),
            blobName,
            bytes
        );
    }

    private sealed class TrackingNonSeekableStream : Stream
    {
        private readonly MemoryStream _stream;

        public int DisposeCount { get; private set; }

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
            return _stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCount++;
                _stream.Dispose();
            }

            base.Dispose(disposing);
        }
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

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
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
