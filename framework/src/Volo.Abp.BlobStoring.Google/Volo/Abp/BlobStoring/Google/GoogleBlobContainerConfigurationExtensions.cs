using System;

namespace Volo.Abp.BlobStoring.Google;

public static class GoogleBlobContainerConfigurationExtensions
{
    public static GoogleBlobProviderConfiguration GetGoogleConfiguration(
        this BlobContainerConfiguration containerConfiguration)
    {
        return new GoogleBlobProviderConfiguration(containerConfiguration);
    }

    public static BlobContainerConfiguration UseGoogle(
        this BlobContainerConfiguration containerConfiguration,
        Action<GoogleBlobProviderConfiguration> azureConfigureAction)
    {
        containerConfiguration.ProviderType = typeof(GoogleBlobProvider);
        containerConfiguration.NamingNormalizers.TryAdd<GoogleBlobNamingNormalizer>();

        azureConfigureAction(new GoogleBlobProviderConfiguration(containerConfiguration));

        return containerConfiguration;
    }
}