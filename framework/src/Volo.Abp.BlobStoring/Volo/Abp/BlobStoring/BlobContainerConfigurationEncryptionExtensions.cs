using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring;

public static class BlobContainerConfigurationEncryptionExtensions
{
    /// <summary>
    /// Enables encryption for the BLOBs of this container. Calling it again is safe:
    /// omitted parameters keep the already configured (or inherited) values,
    /// so multiple modules can compose the configuration.
    /// </summary>
    /// <param name="configuration">The container configuration.</param>
    /// <param name="passPhrase">
    /// Optional container-specific passphrase. Without one, the passphrase is resolved
    /// by the <see cref="IBlobEncryptionKeyProvider"/>. Use
    /// <see cref="ClearEncryptionPassPhrase"/> to remove a configured passphrase.
    /// </param>
    /// <param name="allowLegacyPlaintext">
    /// Allows reading BLOBs stored as plaintext before encryption was enabled:
    /// content without the encrypted format header is then returned as-is,
    /// <b>without any authenticity check</b>. Keep it disabled (default) unless
    /// the container really has such BLOBs.
    /// </param>
    public static BlobContainerConfiguration UseEncryption(
        [NotNull] this BlobContainerConfiguration configuration,
        string? passPhrase = null,
        bool? allowLegacyPlaintext = null)
    {
        Check.NotNull(configuration, nameof(configuration));

        configuration.SetConfiguration(BlobEncryptionConfiguration.EnabledName, true);

        if (allowLegacyPlaintext.HasValue)
        {
            configuration.SetConfiguration(BlobEncryptionConfiguration.AllowLegacyPlaintextName, allowLegacyPlaintext.Value);
        }

        if (passPhrase != null)
        {
            Check.NotNullOrWhiteSpace(passPhrase, nameof(passPhrase));
            configuration.SetConfiguration(BlobEncryptionConfiguration.PassPhraseName, passPhrase);
        }

        return configuration;
    }

    /// <summary>
    /// Removes the container passphrase (including an inherited one), so the
    /// <see cref="IBlobEncryptionKeyProvider"/> resolves the passphrase again.
    /// BLOBs encrypted with the removed passphrase can not be read anymore.
    /// </summary>
    public static BlobContainerConfiguration ClearEncryptionPassPhrase(
        [NotNull] this BlobContainerConfiguration configuration)
    {
        Check.NotNull(configuration, nameof(configuration));

        // An explicit empty value shadows a passphrase inherited from the
        // default (fallback) container configuration.
        configuration.SetConfiguration(BlobEncryptionConfiguration.PassPhraseName, string.Empty);

        return configuration;
    }

    /// <summary>
    /// Disables encryption for this container (even when inherited from the default
    /// configuration) and removes its passphrase/legacy options. Existing encrypted
    /// BLOBs are then returned as stored (still encrypted bytes) while reading.
    /// </summary>
    public static BlobContainerConfiguration DisableEncryption(
        [NotNull] this BlobContainerConfiguration configuration)
    {
        Check.NotNull(configuration, nameof(configuration));

        configuration.SetConfiguration(BlobEncryptionConfiguration.EnabledName, false);
        configuration.ClearConfiguration(BlobEncryptionConfiguration.PassPhraseName);
        configuration.ClearConfiguration(BlobEncryptionConfiguration.AllowLegacyPlaintextName);

        return configuration;
    }
}
