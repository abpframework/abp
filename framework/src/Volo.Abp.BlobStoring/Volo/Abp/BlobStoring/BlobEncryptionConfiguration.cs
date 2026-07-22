namespace Volo.Abp.BlobStoring;

/// <summary>
/// Reads the encryption values of a container configuration (set by the
/// UseEncryption/DisableEncryption extension methods, inherited over the fallback chain).
/// </summary>
internal static class BlobEncryptionConfiguration
{
    public const string EnabledName = "Abp.BlobStoring.Encryption.Enabled";
    public const string PassPhraseName = "Abp.BlobStoring.Encryption.PassPhrase";
    public const string AllowLegacyPlaintextName = "Abp.BlobStoring.Encryption.AllowLegacyPlaintext";

    public static bool IsEnabled(BlobContainerConfiguration configuration)
    {
        return configuration.GetConfigurationOrDefault(EnabledName, false);
    }

    public static string? GetPassPhraseOrNull(BlobContainerConfiguration configuration)
    {
        // An explicit empty value shadows an inherited passphrase (see UseEncryption).
        var passPhrase = configuration.GetConfigurationOrDefault<string?>(PassPhraseName);
        return string.IsNullOrWhiteSpace(passPhrase) ? null : passPhrase;
    }

    public static bool IsLegacyPlaintextAllowed(BlobContainerConfiguration configuration)
    {
        return configuration.GetConfigurationOrDefault(AllowLegacyPlaintextName, false);
    }
}
