using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.BlobStoring.HuaweiCloud
{
    public static class HuaweiCloudBlobContainerConfigurationExtensions
    {
        public static HuaweiCloudBlobProviderConfiguration GetHuaweiCloudConfiguration(
    this BlobContainerConfiguration containerConfiguration)
        {
            return new HuaweiCloudBlobProviderConfiguration(containerConfiguration);
        }

        public static BlobContainerConfiguration UseHuaweiCloud(
            this BlobContainerConfiguration containerConfiguration,
            Action<HuaweiCloudBlobProviderConfiguration> HuaweiCloudConfigureAction)
        {
            containerConfiguration.ProviderType = typeof(HuaweiCloudBlobProvider);
            containerConfiguration.NamingNormalizers.TryAdd<HuaweiCloudBlobNamingNormalizer>();

            HuaweiCloudConfigureAction(new HuaweiCloudBlobProviderConfiguration(containerConfiguration));

            return containerConfiguration;
        }
    }
}
