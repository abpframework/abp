namespace Volo.Abp.BlobStoring;

/// <summary>
/// Global options of the BLOB encryption; enable it per container with the
/// <see cref="BlobContainerConfigurationEncryptionExtensions.UseEncryption"/> extension method.
/// </summary>
public class AbpBlobStoringEncryptionOptions
{
    /// <summary>
    /// The global passphrase, used when no container-specific passphrase is available.
    /// Default: null (encryption must be explicitly keyed).
    /// </summary>
    public string? DefaultPassPhrase { get; set; }

    /// <summary>
    /// PBKDF2 iteration count for newly encrypted BLOBs (existing BLOBs use the count
    /// in their own header). Higher values raise both the offline guessing cost and
    /// the CPU cost of every save/read. Allowed: 100,000 - 600,000. Default: 100,000.
    /// </summary>
    public int KdfIterations { get; set; } = 100_000;
}
