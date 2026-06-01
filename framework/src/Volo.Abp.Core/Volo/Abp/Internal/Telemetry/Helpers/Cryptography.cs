using System;
using System.Security.Cryptography;
using System.Text;

namespace Volo.Abp.Internal.Telemetry.Helpers;

static internal class Cryptography
{
    private const string EncryptionKey = "AbpTelemetryStorageKey"; 

    public static string Encrypt(string plainText)
    {
        Check.NotNullOrEmpty(plainText, nameof(plainText));
        using var aes = Aes.Create();
        using var sha256 = SHA256.Create();

        aes.Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(EncryptionKey));
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;

        var encryptor = aes.CreateEncryptor();
        var inputBytes = Encoding.UTF8.GetBytes(plainText);
        var encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
        return Convert.ToBase64String(encryptedBytes);
    }

    public static string Decrypt(string cipherText)
    {
        Check.NotNullOrEmpty(cipherText, nameof(cipherText));
        using var aes = Aes.Create();
        using var sha256 = SHA256.Create();

        aes.Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(EncryptionKey));
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;

        var decryptor = aes.CreateDecryptor();
        var inputBytes = Convert.FromBase64String(cipherText);
        var decryptedBytes = decryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}