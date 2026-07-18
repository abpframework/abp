using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// Resolves the passphrase used to encrypt/decrypt the BLOBs of a container.
/// </summary>
public interface IBlobEncryptionKeyProvider
{
    /// <summary>
    /// Returns the passphrase to be used, or <c>null</c> if no passphrase is available.
    /// </summary>
    Task<string?> GetPassPhraseOrNullAsync(
        BlobContainerConfiguration configuration,
        CancellationToken cancellationToken = default);
}
