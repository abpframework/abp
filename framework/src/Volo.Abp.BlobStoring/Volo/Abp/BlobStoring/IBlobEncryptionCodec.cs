using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// Encrypts and decrypts the BLOB content stream (authenticated, chunked AES-256-GCM).
/// Replace this service to change the encryption format or algorithm; the built-in
/// <see cref="BlobEncryptionCodec"/> implements version 1 of the format.
/// </summary>
public interface IBlobEncryptionCodec
{
    /// <summary>
    /// Wraps <paramref name="plainStream"/> in a read-only stream that encrypts the
    /// content while it is read. The container and BLOB names are expected in their
    /// normalized form.
    /// </summary>
    Task<Stream> CreateEncryptingStreamAsync(
        [NotNull] BlobContainerConfiguration configuration,
        [NotNull] string containerName,
        [NotNull] string blobName,
        Guid? tenantId,
        [NotNull] Stream plainStream,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Wraps <paramref name="cipherStream"/> in a read-only stream that decrypts the
    /// content while it is read. The container and BLOB names are expected in their
    /// normalized form.
    /// </summary>
    Task<Stream> CreateDecryptingStreamAsync(
        [NotNull] BlobContainerConfiguration configuration,
        [NotNull] string containerName,
        [NotNull] string blobName,
        Guid? tenantId,
        [NotNull] Stream cipherStream,
        CancellationToken cancellationToken = default);
}
