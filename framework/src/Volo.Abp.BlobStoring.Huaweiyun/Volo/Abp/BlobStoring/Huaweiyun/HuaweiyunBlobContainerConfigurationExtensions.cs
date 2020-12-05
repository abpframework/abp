using System;

namespace Volo.Abp.BlobStoring.Huaweiyun
{
    public static class HuaweiyunBlobContainerConfigurationExtensions
    {
        public static HuaweiyunBlobProviderConfiguration GetHuaweiyunConfiguration(
                this BlobContainerConfiguration containerConfiguration)
        {
            return new HuaweiyunBlobProviderConfiguration(containerConfiguration);
        }

        public static BlobContainerConfiguration UseHuaweiyun(
            this BlobContainerConfiguration containerConfiguration,
            Action<HuaweiyunBlobProviderConfiguration> huaweiyunConfigureAction)
        {
            containerConfiguration.ProviderType = typeof(HuaweiyunBlobProvider);
            containerConfiguration.NamingNormalizers.TryAdd<HuaweiyunBlobNamingNormalizer>();

            huaweiyunConfigureAction(new HuaweiyunBlobProviderConfiguration(containerConfiguration));

            return containerConfiguration;
        }
    }
}