namespace Volo.Abp.BlobStoring;

public static class BlobStoringEncryptionConfigurationNames
{
    /// <summary>
    /// Configuration name used to store a container-specific encryption passphrase
    /// on <see cref="BlobContainerConfiguration"/>.
    /// </summary>
    public const string PassPhrase = "Abp.BlobStoring.Encryption.PassPhrase";
}
