using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.BlobStoring.HuaweiCloud
{
    public class HuaweiCloudBlobProviderConfiguration
    {
        public string BucketName
        {
            get => _containerConfiguration.GetConfigurationOrDefault<string>(HuaweiCloudBlobProviderConfigurationNames.BucketName);
            set => _containerConfiguration.SetConfiguration(HuaweiCloudBlobProviderConfigurationNames.BucketName, value);
        }

        /// <summary>
        /// endPoint is an URL, domain name, IPv4 address or IPv6 address.
        /// </summary>
        public string EndPoint
        {
            get => _containerConfiguration.GetConfiguration<string>(HuaweiCloudBlobProviderConfigurationNames.EndPoint);
            set => _containerConfiguration.SetConfiguration(HuaweiCloudBlobProviderConfigurationNames.EndPoint, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        /// <summary>
        /// accessKey is like user-id that uniquely identifies your account.This field is optional and can be omitted for anonymous access.
        /// </summary>
        public string AccessKey
        {
            get => _containerConfiguration.GetConfiguration<string>(HuaweiCloudBlobProviderConfigurationNames.AccessKey);
            set => _containerConfiguration.SetConfiguration(HuaweiCloudBlobProviderConfigurationNames.AccessKey, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        /// <summary>
        /// secretKey is the password to your account.This field is optional and can be omitted for anonymous access.
        /// </summary>
        public string SecretKey
        {
            get => _containerConfiguration.GetConfiguration<string>(HuaweiCloudBlobProviderConfigurationNames.SecretKey);
            set => _containerConfiguration.SetConfiguration(HuaweiCloudBlobProviderConfigurationNames.SecretKey, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

       

        /// <summary>
        ///Default value: false.
        /// </summary>
        public bool CreateBucketIfNotExists
        {
            get => _containerConfiguration.GetConfigurationOrDefault(HuaweiCloudBlobProviderConfigurationNames.CreateBucketIfNotExists, false);
            set => _containerConfiguration.SetConfiguration(HuaweiCloudBlobProviderConfigurationNames.CreateBucketIfNotExists, value);
        }

        private readonly BlobContainerConfiguration _containerConfiguration;

        public HuaweiCloudBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }
    }
}
