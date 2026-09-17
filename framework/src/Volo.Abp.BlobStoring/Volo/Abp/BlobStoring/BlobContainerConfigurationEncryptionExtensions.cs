using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring;

public static class BlobContainerConfigurationEncryptionExtensions
{
    /// <summary>
    /// Enables encryption for the BLOBs of this container. Calling it again is safe:
    /// omitted parameters keep the already configured (or inherited) values,
    /// so multiple modules can compose the configuration.
    /// </summary>
    /// <param name="containerConfiguration">The container configuration.</param>
    /// <param name="passPhrase">
    /// Optional container-specific passphrase. Without one, the passphrase is resolved
    /// by the <see cref="IBlobEncryptionKeyProvider"/>. Use
    /// <see cref="ClearEncryptionPassPhrase"/> to remove a configured passphrase.
    /// </param>
    /// <param name="allowLegacyPlainText">
    /// Allows reading BLOBs stored as plaintext before encryption was enabled:
    /// content without the encrypted format header is then returned as-is,
    /// <b>without any authenticity check</b>. Keep it disabled (default) unless
    /// the container really has such BLOBs.
    /// </param>
    public static BlobContainerConfiguration UseEncryption(
        [NotNull] this BlobContainerConfiguration containerConfiguration,
        string? passPhrase = null,
        bool? allowLegacyPlainText = null)
    {
        Check.NotNull(containerConfiguration, nameof(containerConfiguration));

        // Validate all arguments before touching the configuration, so a failed
        // call does not leave it partially modified
        if (passPhrase != null)
        {
            Check.NotNullOrWhiteSpace(passPhrase, nameof(passPhrase));
        }

        containerConfiguration.SetConfiguration(BlobEncryptionConfigurationNames.Enabled, true);

        if (allowLegacyPlainText.HasValue)
        {
            containerConfiguration.SetConfiguration(BlobEncryptionConfigurationNames.AllowLegacyPlainText, allowLegacyPlainText.Value);
        }

        if (passPhrase != null)
        {
            containerConfiguration.SetConfiguration(BlobEncryptionConfigurationNames.PassPhrase, passPhrase);
        }

        return containerConfiguration;
    }

    /// <summary>
    /// Indicates whether encryption is enabled for this container (own or inherited
    /// configuration). Storage providers can use it to detect a transformed content stream.
    /// </summary>
    public static bool IsEncryptionEnabled([NotNull] this BlobContainerConfiguration containerConfiguration)
    {
        Check.NotNull(containerConfiguration, nameof(containerConfiguration));

        return BlobEncryptionConfiguration.IsEnabled(containerConfiguration);
    }

    /// <summary>
    /// Removes the container passphrase (including an inherited one), so the
    /// <see cref="IBlobEncryptionKeyProvider"/> resolves the passphrase again.
    /// BLOBs encrypted with the removed passphrase can not be read anymore.
    /// </summary>
    public static BlobContainerConfiguration ClearEncryptionPassPhrase(
        [NotNull] this BlobContainerConfiguration containerConfiguration)
    {
        Check.NotNull(containerConfiguration, nameof(containerConfiguration));

        // An explicit empty value shadows a passphrase inherited from the
        // default (fallback) container configuration.
        containerConfiguration.SetConfiguration(BlobEncryptionConfigurationNames.PassPhrase, string.Empty);

        return containerConfiguration;
    }

    /// <summary>
    /// Disables encryption for this container (even when inherited from the default
    /// configuration) and removes its own passphrase/legacy options. Existing encrypted
    /// BLOBs are then returned as stored (still encrypted bytes) while reading — unless
    /// the container also has pipeline contributors, which still run and typically fail
    /// on the ciphertext.
    /// </summary>
    public static BlobContainerConfiguration DisableEncryption(
        [NotNull] this BlobContainerConfiguration containerConfiguration)
    {
        Check.NotNull(containerConfiguration, nameof(containerConfiguration));

        containerConfiguration.SetConfiguration(BlobEncryptionConfigurationNames.Enabled, false);
        containerConfiguration.ClearConfiguration(BlobEncryptionConfigurationNames.PassPhrase);
        containerConfiguration.ClearConfiguration(BlobEncryptionConfigurationNames.AllowLegacyPlainText);

        return containerConfiguration;
    }
}
