using System;

namespace Volo.Abp.BlobStoring.Azure
{
    public static class AzureBlobContainerConfigurationExtensions
    {
        public static AzureBlobProviderConfiguration GetAzureConfiguration(
            this BlobContainerConfiguration containerConfiguration)
        {
            return new AzureBlobProviderConfiguration(containerConfiguration);
        }

        public static BlobContainerConfiguration UseAzure(
            this BlobContainerConfiguration containerConfiguration,
            Action<AzureBlobProviderConfiguration> fileSystemConfigureAction)
        {
            containerConfiguration.ProviderType = typeof(AzureBlobProvider);

            fileSystemConfigureAction(new AzureBlobProviderConfiguration(containerConfiguration));

            return containerConfiguration;
        }
    }
}
