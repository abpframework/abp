using System;

namespace Volo.Abp.BlobStoring.Aws
{
    public static class AwsBlobContainerConfigurationExtensions
    {
        public static AwsBlobProviderConfiguration GetAwsConfiguration(
            this BlobContainerConfiguration containerConfiguration)
        {
            return new AwsBlobProviderConfiguration(containerConfiguration);
        }

        public static BlobContainerConfiguration UseAws(
            this BlobContainerConfiguration containerConfiguration,
            Action<AwsBlobProviderConfiguration> awsConfigureAction)
        {
            containerConfiguration.ProviderType = typeof(AwsBlobProvider);
            containerConfiguration.NamingNormalizers.TryAdd<AwsBlobNamingNormalizer>();

            awsConfigureAction(new AwsBlobProviderConfiguration(containerConfiguration));

            return containerConfiguration;
        }
    }
}
