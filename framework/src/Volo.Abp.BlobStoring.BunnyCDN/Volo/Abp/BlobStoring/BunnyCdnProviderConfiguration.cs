using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring.BunnyCDN.Volo.Abp.BlobStoring
{
    public class BunnyCdnProviderConfiguration
    {
        private readonly BlobContainerConfiguration containerConfiguration;

        public BunnyCdnProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            this.containerConfiguration = containerConfiguration;
        }

        /// <summary>
        /// Gets or sets Storage Zone Name.
        /// </summary>
        public string StorageZoneName
        {
            get => GetConfiguration<string>();
            set => SetConfiguration(value);
        }

        /// <summary>
        /// Gets or sets Api Access Key. Also known as password of StorageZone.
        /// </summary>
        public string ApiAccessKey
        {
            get => GetConfiguration<string>();
            set => SetConfiguration(value);
        }

        /// <summary>
        /// Gets or sets Main Application Region. This is an optional parameter.
        /// </summary>
        public string MainApplicationRegion
        {
            get => GetConfiguration<string>();
            set => SetConfiguration(value);
        }

        /// <summary>
        /// Gets or sets Base URL. By default, generates BunnyCDN url. You can override if you're using PullZone.
        /// </summary>
        public string BaseUrl
        {
            get => GetConfigurationOrDefault<string>(null) ?? $"https://{StorageZoneName}.b-cdn.net";
            set => SetConfiguration(value);
        }

        private void SetConfiguration<T>(T value, [CallerMemberName] string name = "")
        {
            containerConfiguration.SetConfiguration(BunnyCdnProviderConfigurationConsts.ConfigurationPrefix + name, value);
        }

        private T GetConfigurationOrDefault<T>(T defaultValue = default, [CallerMemberName] string name = "")
        {
            return containerConfiguration.GetConfigurationOrDefault(BunnyCdnProviderConfigurationConsts.ConfigurationPrefix + name, defaultValue);
        }

        private T GetConfiguration<T>([CallerMemberName] string name = "")
        {
            return containerConfiguration.GetConfiguration<T>(BunnyCdnProviderConfigurationConsts.ConfigurationPrefix + name);
        }
    }
}
