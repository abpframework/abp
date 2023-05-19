using JetBrains.Annotations;

namespace Volo.Abp.Security.Encryption;

/// <summary>
/// Can be used to simply encrypt/decrypt texts.
/// Use <see cref="AbpStringEncryptionOptions"/> to configure default values.
/// </summary>
public interface IStringEncryptionService
{
    /// <summary>
    /// Encrypts a text.
    /// </summary>
    /// <param name="plainText">The text in plain format</param>
    /// <param name="passPhrase">A phrase to use as the encryption key (optional, uses default if not provided)</param>
    /// <param name="salt">Salt value (optional, uses default if not provided)</param>
    /// <returns>Enrypted text</returns>
    [CanBeNull]
    string Encrypt([CanBeNull] string plainText, string passPhrase = null, byte[] salt = null);

    /// <summary>
    /// Decrypts a text that is encrypted by the <see cref="Encrypt"/> method.
    /// </summary>
    /// <param name="cipherText">The text in encrypted format</param>
    /// <param name="passPhrase">A phrase to use as the encryption key (optional, uses default if not provided)</param>
    /// <param name="salt">Salt value (optional, uses default if not provided)</param>
    /// <returns>Decrypted text</returns>
    [CanBeNull]
    string Decrypt([CanBeNull] string cipherText, string passPhrase = null, byte[] salt = null);
}
