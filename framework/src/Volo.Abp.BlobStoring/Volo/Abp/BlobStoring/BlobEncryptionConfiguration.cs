namespace Volo.Abp.BlobStoring;

/// <summary>
/// Reads the encryption values of a container configuration (set by the
/// UseEncryption/DisableEncryption extension methods, inherited over the fallback chain).
/// </summary>
internal static class BlobEncryptionConfiguration
{
    public static bool IsEnabled(BlobContainerConfiguration configuration)
    {
        return configuration.GetConfigurationOrDefault(BlobEncryptionConfigurationNames.Enabled, false);
    }

    public static string? GetPassPhraseOrNull(BlobContainerConfiguration configuration)
    {
        // An explicit empty value shadows an inherited passphrase (see UseEncryption).
        var passPhrase = configuration.GetConfigurationOrDefault<string?>(BlobEncryptionConfigurationNames.PassPhrase);
        return string.IsNullOrWhiteSpace(passPhrase) ? null : passPhrase;
    }

    public static bool IsLegacyPlainTextAllowed(BlobContainerConfiguration configuration)
    {
        return configuration.GetConfigurationOrDefault(BlobEncryptionConfigurationNames.AllowLegacyPlainText, false);
    }
}
