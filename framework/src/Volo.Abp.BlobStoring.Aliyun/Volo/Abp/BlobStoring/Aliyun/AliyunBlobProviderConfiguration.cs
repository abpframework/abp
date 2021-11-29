using System;

namespace Volo.Abp.BlobStoring.Aliyun
{
    /// <summary>
    /// Sub-account access to OSS or STS temporary authorization to access OSS
    /// </summary>
    public class AliyunBlobProviderConfiguration
    {
        public string AccessKeyId
        {
            get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.AccessKeyId);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.AccessKeyId, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        public string AccessKeySecret
        {
            get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.AccessKeySecret);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.AccessKeySecret, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        public string Endpoint
        {
            get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.Endpoint);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.Endpoint, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        public bool UseSecurityTokenService
        {
            get => _containerConfiguration.GetConfigurationOrDefault(AliyunBlobProviderConfigurationNames.UseSecurityTokenService, false);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.UseSecurityTokenService, value);
        }

        public string RegionId
        {
            get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.RegionId);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.RegionId, value);
        }

        /// <summary>
        /// acs:ram::$accountID:role/$roleName
        /// </summary>
        public string RoleArn
        {
            get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.RoleArn);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.RoleArn, value);
        }

        /// <summary>
        /// The name used to identify the temporary access credentials, it is recommended to use different application users to distinguish.
        /// </summary>
        public string RoleSessionName
        {
            get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.RoleSessionName);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.RoleSessionName, value);
        }

        /// <summary>
        /// Set the validity period of the temporary access credential, the unit is s, the minimum is 900, and the maximum is 3600.
        /// </summary>
        public int DurationSeconds
        {
            get => _containerConfiguration.GetConfigurationOrDefault(AliyunBlobProviderConfigurationNames.DurationSeconds, 0);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.DurationSeconds, value);
        }

        /// <summary>
        /// If policy is empty, the user will get all permissions under this role
        /// </summary>
        public string Policy
        {
            get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.Policy);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.Policy, value);
        }

        /// <summary>
        /// This name may only contain lowercase letters, numbers, and hyphens, and must begin with a letter or a number.
        /// Each hyphen must be preceded and followed by a non-hyphen character.
        /// The name must also be between 3 and 63 characters long.
        /// If this parameter is not specified, the ContainerName of the <see cref="BlobProviderArgs"/> will be used.
        /// </summary>
        public string ContainerName
        {
            get => _containerConfiguration.GetConfigurationOrDefault<string>(AliyunBlobProviderConfigurationNames.ContainerName);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.ContainerName, value);
        }

        /// <summary>
        /// Default value: false.
        /// </summary>
        public bool CreateContainerIfNotExists
        {
            get => _containerConfiguration.GetConfigurationOrDefault(AliyunBlobProviderConfigurationNames.CreateContainerIfNotExists, false);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.CreateContainerIfNotExists, value);
        }

        private readonly string _temporaryCredentialsCacheKey;
        public string TemporaryCredentialsCacheKey
        {
            get => _containerConfiguration.GetConfigurationOrDefault(AliyunBlobProviderConfigurationNames.TemporaryCredentialsCacheKey, _temporaryCredentialsCacheKey);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.TemporaryCredentialsCacheKey, value);
        }

        private readonly BlobContainerConfiguration _containerConfiguration;

        public AliyunBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
            _temporaryCredentialsCacheKey = Guid.NewGuid().ToString("N");
        }
    }
}
