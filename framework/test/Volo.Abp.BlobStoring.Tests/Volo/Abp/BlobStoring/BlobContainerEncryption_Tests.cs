#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
    private readonly IBlobContainer<TestContainer6> _container6; // UseEncryption("container6-passphrase", allowLegacyPlaintext: true)
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
        using var cts = new System.Threading.CancellationTokenSource();
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

        using var cts = new System.Threading.CancellationTokenSource();
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
            await defaultProvider.ResolveForDecryptionAsync(BlobEncryptionKeySource.Tenant, new BlobContainerConfiguration());
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
            await defaultProvider.ResolveForEncryptionAsync(configuration);
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
        SetRawBytes<TestContainer6>(blobName, tinyContent); // allowLegacyPlaintext: true

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
        Should.Throw<AbpException>(() => BlobEncryptionCodec.CreateChunkNonce(new byte[8], -1));
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
    public void Should_Keep_The_V1_Format_Stable()
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
