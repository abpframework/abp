using System;
using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// The identity of the BLOB an encryption key is resolved for. It lets a custom
/// <see cref="IBlobEncryptionKeyProvider"/> select the key by the container, the
/// BLOB name or the tenant — not only by the container configuration.
/// </summary>
public class BlobEncryptionKeyContext
{
    /// <summary>
    /// The configuration of the container the BLOB belongs to (with the container
    /// passphrase, if one was set with <c>UseEncryption</c>).
    /// </summary>
    [NotNull]
    public BlobContainerConfiguration Configuration { get; }

    /// <summary>
    /// The normalized container name.
    /// </summary>
    [NotNull]
    public string ContainerName { get; }

    /// <summary>
    /// The normalized BLOB name.
    /// </summary>
    [NotNull]
    public string BlobName { get; }

    /// <summary>
    /// The tenant of the BLOB operation (null for the host or a shared container).
    /// </summary>
    public Guid? TenantId { get; }

    /// <summary>
    /// Creates the context; the names are expected in their normalized form.
    /// </summary>
    public BlobEncryptionKeyContext(
        [NotNull] BlobContainerConfiguration configuration,
        [NotNull] string containerName,
        [NotNull] string blobName,
        Guid? tenantId)
    {
        Configuration = Check.NotNull(configuration, nameof(configuration));
        ContainerName = Check.NotNullOrWhiteSpace(containerName, nameof(containerName));
        BlobName = Check.NotNullOrWhiteSpace(blobName, nameof(blobName));
        TenantId = tenantId;
    }
}
