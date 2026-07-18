using System.Text;

namespace Volo.Abp.Security.Encryption;

/// <summary>
/// Options used by <see cref="IByteArrayEncryptionService"/>.
/// These options are independent from <see cref="AbpStringEncryptionOptions"/>;
/// changing them does not affect <see cref="IStringEncryptionService"/>.
/// </summary>
public class AbpByteArrayEncryptionOptions
{
    /// <summary>
    /// Default password to encrypt/decrypt data.
    /// It's recommended to set to another value for security.
    /// Default value: "x9V4qL2mZ8sT1pRe"
    /// </summary>
    public string DefaultPassPhrase { get; set; }

    /// <summary>
    /// This constant string is used as a "salt" value for the key derivation function calls.
    /// Default value: Encoding.ASCII.GetBytes("kT8!qW2e")
    /// </summary>
    public byte[] DefaultSalt { get; set; }

    /// <summary>
    /// Iteration count of the PBKDF2 key derivation function.
    /// Default value: 100000.
    /// WARNING: Changing this value makes previously encrypted data undecryptable,
    /// since the key is derived again with the current value during decryption.
    /// </summary>
    public int DeriveBytesIterations { get; set; }

    /// <summary>
    /// Size (in bytes) of the plaintext chunks that are encrypted and authenticated
    /// one by one while processing streams. Larger data is processed in constant memory,
    /// independent from the total data size.
    /// Default value: 65536 (64 KB).
    /// Maximum value: 16777216 (16 MB).
    /// </summary>
    public int ChunkSize { get; set; }

    public AbpByteArrayEncryptionOptions()
    {
        DefaultPassPhrase = "x9V4qL2mZ8sT1pRe";
        DefaultSalt = Encoding.ASCII.GetBytes("kT8!qW2e");
        DeriveBytesIterations = 100000;
        ChunkSize = 64 * 1024;
    }
}
