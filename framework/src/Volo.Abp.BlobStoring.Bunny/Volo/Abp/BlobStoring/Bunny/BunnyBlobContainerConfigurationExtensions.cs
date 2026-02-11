using System;

namespace Volo.Abp.BlobStoring.Bunny;

public static class BunnyBlobContainerConfigurationExtensions
{
    public static BunnyBlobProviderConfiguration GetBunnyConfiguration(
        this BlobContainerConfiguration containerConfiguration)
    {
        return new BunnyBlobProviderConfiguration(containerConfiguration);
    }

    public static BlobContainerConfiguration UseBunny(
        this BlobContainerConfiguration containerConfiguration,
        Action<BunnyBlobProviderConfiguration> bunnyConfigureAction)
    {
        containerConfiguration.ProviderType = typeof(BunnyBlobProvider);
        containerConfiguration.NamingNormalizers.TryAdd<BunnyBlobNamingNormalizer>();

        bunnyConfigureAction(new BunnyBlobProviderConfiguration(containerConfiguration));

        return containerConfiguration;
    }
}