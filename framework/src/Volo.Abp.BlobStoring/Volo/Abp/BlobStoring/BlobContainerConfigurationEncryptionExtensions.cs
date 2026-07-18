using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring;

public static class BlobContainerConfigurationEncryptionExtensions
{
    /// <summary>
    /// Enables encryption for the BLOBs of this container.
    /// </summary>
    /// <param name="configuration">The container configuration.</param>
    /// <param name="passPhrase">
    /// Optional container-specific passphrase. When not given, the passphrase is resolved
    /// by the <see cref="IBlobEncryptionKeyProvider"/> (tenant-specific setting first,
    /// then <see cref="AbpBlobStoringEncryptionOptions.DefaultPassPhrase"/>).
    /// </param>
    public static BlobContainerConfiguration UseEncryption(
        [NotNull] this BlobContainerConfiguration configuration,
        string? passPhrase = null)
    {
        Check.NotNull(configuration, nameof(configuration));

        if (!configuration.PipelineContributors.Contains(typeof(BlobEncryptionContributor)))
        {
            configuration.PipelineContributors.Add(typeof(BlobEncryptionContributor));
        }

        if (passPhrase != null)
        {
            configuration.SetConfiguration(BlobStoringEncryptionConfigurationNames.PassPhrase, passPhrase);
        }

        return configuration;
    }
}
