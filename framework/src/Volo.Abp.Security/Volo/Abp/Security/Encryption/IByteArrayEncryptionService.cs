using System.IO;

namespace Volo.Abp.Security.Encryption;

/// <summary>
/// Can be used to encrypt/decrypt binary data (files, images, serialized objects etc.)
/// with authenticated encryption.
/// Use <see cref="AbpByteArrayEncryptionOptions"/> to configure default values.
/// This service is independent from <see cref="IStringEncryptionService"/>;
/// data encrypted by one of them can not be decrypted by the other.
/// </summary>
public interface IByteArrayEncryptionService
{
    /// <summary>
    /// Encrypts binary data.
    /// </summary>
    /// <param name="plainBytes">The data in plain format</param>
    /// <param name="passPhrase">A phrase to use as the encryption key (optional, uses default if not provided)</param>
    /// <param name="salt">Salt value (optional, uses default if not provided)</param>
    /// <returns>Encrypted data, including a format header and authentication tags</returns>
    byte[]? Encrypt(byte[]? plainBytes, string? passPhrase = null, byte[]? salt = null);

    /// <summary>
    /// Decrypts binary data that is encrypted by the <see cref="Encrypt(byte[], string?, byte[])"/> method.
    /// </summary>
    /// <param name="cipherBytes">The data in encrypted format</param>
    /// <param name="passPhrase">A phrase to use as the encryption key (optional, uses default if not provided)</param>
    /// <param name="salt">Salt value (optional, uses default if not provided)</param>
    /// <returns>Decrypted data</returns>
    /// <exception cref="System.Security.Cryptography.CryptographicException">
    /// Thrown when the data is tampered, corrupted or the passphrase/salt is wrong.
    /// </exception>
    byte[]? Decrypt(byte[]? cipherBytes, string? passPhrase = null, byte[]? salt = null);

    /// <summary>
    /// Encrypts a stream into another stream. The data is processed in chunks,
    /// so the memory usage is constant and independent from the total data size.
    /// Each chunk is authenticated before the next one is written.
    /// </summary>
    /// <param name="plainStream">The stream to read the plain data from</param>
    /// <param name="cipherStream">The stream to write the encrypted data to</param>
    /// <param name="passPhrase">A phrase to use as the encryption key (optional, uses default if not provided)</param>
    /// <param name="salt">Salt value (optional, uses default if not provided)</param>
    void Encrypt(Stream plainStream, Stream cipherStream, string? passPhrase = null, byte[]? salt = null);

    /// <summary>
    /// Decrypts a stream that is encrypted by the <see cref="Encrypt(Stream, Stream, string?, byte[])"/> method.
    /// Each chunk's authentication tag is verified before its plaintext is written
    /// to the <paramref name="plainStream"/>, so tampered data is never released.
    /// </summary>
    /// <param name="cipherStream">The stream to read the encrypted data from</param>
    /// <param name="plainStream">The stream to write the decrypted data to</param>
    /// <param name="passPhrase">A phrase to use as the encryption key (optional, uses default if not provided)</param>
    /// <param name="salt">Salt value (optional, uses default if not provided)</param>
    /// <exception cref="System.Security.Cryptography.CryptographicException">
    /// Thrown when the data is tampered, corrupted or the passphrase/salt is wrong.
    /// </exception>
    void Decrypt(Stream cipherStream, Stream plainStream, string? passPhrase = null, byte[]? salt = null);
}
