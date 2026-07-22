namespace Volo.Abp.BlobStoring;

/// <summary>
/// Identifies where the encryption passphrase of a BLOB comes from. The value is
/// stored in the BLOB header, so decryption uses the same source again even if
/// other sources are configured later.
/// </summary>
public enum BlobEncryptionKeySource : byte
{
    /// <summary>The container-specific passphrase (see the UseEncryption extension method).</summary>
    Container = 1,

    /// <summary>A tenant-specific passphrase, provided by a custom <see cref="IBlobEncryptionKeyProvider"/>; unused by the default provider.</summary>
    Tenant = 2,

    /// <summary>The global passphrase (see <see cref="AbpBlobStoringEncryptionOptions.DefaultPassPhrase"/>).</summary>
    Global = 3
}
