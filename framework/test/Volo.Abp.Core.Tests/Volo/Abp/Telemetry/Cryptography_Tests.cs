using System;
using Shouldly;
using Volo.Abp.Internal.Telemetry.Helpers;
using Xunit;

namespace Volo.Abp.Telemetry;

public class Cryptography_Tests
{
    [Fact]
    public void Should_Encrypt_And_Decrypt_Text_Successfully()
    {
        // Arrange
        const string plainText = "Test message 123!";

        // Act
        var encryptedText = Cryptography.Encrypt(plainText);
        var decryptedText = Cryptography.Decrypt(encryptedText);

        // Assert
        decryptedText.ShouldBe(plainText);
    }

    [Fact]
    public void Should_Generate_Different_Encrypted_Values_For_Different_Inputs()
    {
        // Arrange
        const string text1 = "Message 1";
        const string text2 = "Message 2";

        // Act
        var encrypted1 = Cryptography.Encrypt(text1);
        var encrypted2 = Cryptography.Encrypt(text2);

        // Assert
        encrypted1.ShouldNotBe(encrypted2);
    }
    [Fact]
    public void Encrypt_Should_Throw_ArgumentException_When_Input_Is_Null()
    {
        // Act & Assert
         Should.Throw<ArgumentException>(() => Cryptography.Encrypt(null));
    }

    [Fact]
    public void Decrypt_Should_Throw_ArgumentException_When_Input_Is_Null()
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => Cryptography.Decrypt(null));
    }

}