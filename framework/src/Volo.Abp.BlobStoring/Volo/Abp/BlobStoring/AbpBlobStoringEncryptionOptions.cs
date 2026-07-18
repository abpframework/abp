namespace Volo.Abp.BlobStoring;

public class AbpBlobStoringEncryptionOptions
{
    /// <summary>
    /// The global passphrase, used when no container-specific or
    /// tenant-specific passphrase is available.
    /// Default: null (encryption must be explicitly keyed).
    /// </summary>
    public string? DefaultPassPhrase { get; set; }
}
