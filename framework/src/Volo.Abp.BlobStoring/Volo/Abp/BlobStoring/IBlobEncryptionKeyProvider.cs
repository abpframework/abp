using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// Resolves the passphrase used to encrypt/decrypt the BLOBs of a container.
/// Replace this service to read the passphrases from another source, like a vault
/// or another secret store (the provider must be able to return the passphrase
/// itself; hardware-backed non-exportable keys are not supported).
/// </summary>
public interface IBlobEncryptionKeyProvider
{
    /// <summary>
    /// Resolves the passphrase (and its source) to encrypt a new BLOB; throws if none is available.
    /// </summary>
    Task<BlobEncryptionKey> ResolveForEncryptionAsync(
        BlobContainerConfiguration configuration,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Resolves the passphrase for the key source recorded in the BLOB header;
    /// throws if it is not available anymore.
    /// </summary>
    Task<string> ResolveForDecryptionAsync(
        BlobEncryptionKeySource keySource,
        BlobContainerConfiguration configuration,
        CancellationToken cancellationToken = default);
}
