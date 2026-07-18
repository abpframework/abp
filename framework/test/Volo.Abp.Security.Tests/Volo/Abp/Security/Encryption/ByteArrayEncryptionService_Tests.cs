using System;
using System.IO;
using System.Security.Cryptography;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Security.Encryption;

public class ByteArrayEncryptionService_Tests : AbpIntegratedTest<AbpSecurityTestModule>
{
    private readonly IByteArrayEncryptionService _byteArrayEncryptionService;

    public ByteArrayEncryptionService_Tests()
    {
        _byteArrayEncryptionService = GetRequiredService<IByteArrayEncryptionService>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData(new byte[0])]
    [InlineData(new byte[] { 1, 2, 3, 42, 255 })]
    public void Should_Encrypt_And_Decrypt_With_Default_Options(byte[] plainBytes)
    {
        _byteArrayEncryptionService
            .Decrypt(_byteArrayEncryptionService.Encrypt(plainBytes))
            .ShouldBe(plainBytes);
    }

    [Fact]
    public void Should_Encrypt_And_Decrypt_Large_Data()
    {
        var plainBytes = new byte[2 * 1024 * 1024]; // 2 MB
        new Random(42).NextBytes(plainBytes);

        var cipherBytes = _byteArrayEncryptionService.Encrypt(plainBytes);

        cipherBytes.ShouldNotBeNull();
        cipherBytes.ShouldNotBe(plainBytes);

        _byteArrayEncryptionService.Decrypt(cipherBytes).ShouldBe(plainBytes);
    }

    [Fact]
    public void Should_Encrypt_And_Decrypt_Streams()
    {
        var plainBytes = new byte[2 * 1024 * 1024]; // 2 MB, spans multiple chunks
        new Random(42).NextBytes(plainBytes);

        using var plainInput = new MemoryStream(plainBytes);
        using var cipherOutput = new MemoryStream();
        _byteArrayEncryptionService.Encrypt(plainInput, cipherOutput);

        cipherOutput.Position = 0;
        using var plainOutput = new MemoryStream();
        _byteArrayEncryptionService.Decrypt(cipherOutput, plainOutput);

        plainOutput.ToArray().ShouldBe(plainBytes);
    }

    [Fact]
    public void Should_Produce_Different_Output_For_The_Same_Input()
    {
        var plainBytes = new byte[] { 1, 2, 3, 42, 255 };

        var cipherBytes1 = _byteArrayEncryptionService.Encrypt(plainBytes);
        var cipherBytes2 = _byteArrayEncryptionService.Encrypt(plainBytes);

        cipherBytes1.ShouldNotBe(cipherBytes2);
    }

    [Fact]
    public void Should_Write_Format_Header()
    {
        var cipherBytes = _byteArrayEncryptionService.Encrypt(new byte[] { 1, 2, 3 });

        cipherBytes.ShouldNotBeNull();
        cipherBytes.Length.ShouldBeGreaterThan(14);
        cipherBytes[0].ShouldBe((byte)1); // Format version
    }

    [Fact]
    public void Should_Fail_To_Decrypt_Tampered_Data()
    {
        var cipherBytes = _byteArrayEncryptionService.Encrypt(new byte[] { 1, 2, 3, 42, 255 });

        cipherBytes![cipherBytes.Length - 1] ^= 0xFF; // Flip the last byte (inside the auth tag)

        Assert.ThrowsAny<CryptographicException>(() => _byteArrayEncryptionService.Decrypt(cipherBytes));
    }

    [Fact]
    public void Should_Fail_To_Decrypt_With_Wrong_PassPhrase()
    {
        var cipherBytes = _byteArrayEncryptionService.Encrypt(new byte[] { 1, 2, 3 }, "passphrase-1");

        Assert.ThrowsAny<CryptographicException>(() => _byteArrayEncryptionService.Decrypt(cipherBytes, "passphrase-2"));
    }

    [Fact]
    public void Should_Encrypt_And_Decrypt_With_Custom_PassPhrase_And_Salt()
    {
        var plainBytes = new byte[] { 1, 2, 3, 42, 255 };
        var salt = new byte[] { 9, 8, 7, 6, 5, 4, 3, 2 };

        _byteArrayEncryptionService
            .Decrypt(_byteArrayEncryptionService.Encrypt(plainBytes, "my-passphrase", salt), "my-passphrase", salt)
            .ShouldBe(plainBytes);
    }

    [Fact]
    public void Should_Return_Null_For_Null_Or_Empty_CipherBytes()
    {
        _byteArrayEncryptionService.Decrypt(null).ShouldBeNull();
        _byteArrayEncryptionService.Decrypt(new byte[0]).ShouldBeNull();
    }

    [Fact]
    public void Should_Fail_When_Authenticated_Terminal_Record_Is_Missing()
    {
        var plainBytes = new byte[128 * 1024];
        var cipherBytes = _byteArrayEncryptionService.Encrypt(plainBytes)!;

        Array.Resize(ref cipherBytes, cipherBytes.Length - 20); // 4-byte terminal marker + 16-byte GCM tag

        Should.Throw<AbpException>(() => _byteArrayEncryptionService.Decrypt(cipherBytes));
    }

    [Fact]
    public void Should_Reject_Oversized_Encoded_Chunk_Size_Before_Allocating()
    {
        var cipherBytes = _byteArrayEncryptionService.Encrypt(new byte[] { 1 })!;
        cipherBytes[2] = 0x7F;
        cipherBytes[3] = 0xFF;
        cipherBytes[4] = 0xFF;
        cipherBytes[5] = 0xFF;

        Should.Throw<AbpException>(() => _byteArrayEncryptionService.Decrypt(cipherBytes));
    }
}
