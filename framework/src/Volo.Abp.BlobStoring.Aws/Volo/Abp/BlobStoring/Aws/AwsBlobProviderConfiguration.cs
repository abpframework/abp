using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;

namespace Volo.Abp.BlobStoring.Aws
{
    public class AwsBlobProviderConfiguration
    {
        public string AccessKeyId
        {
            get => _containerConfiguration.GetConfiguration<string>(AwsBlobProviderConfigurationNames.AccessKeyId);
            set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.AccessKeyId, value);
        }

        public string SecretAccessKey
        {
            get => _containerConfiguration.GetConfiguration<string>(AwsBlobProviderConfigurationNames.SecretAccessKey);
            set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.SecretAccessKey, value);
        }

        public bool UseAwsCredentials
        {
            get => _containerConfiguration.GetConfiguration<bool>(AwsBlobProviderConfigurationNames.UseAwsCredentials);
            set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.UseAwsCredentials, value);
        }

        public bool UseTemporaryCredentials
        {
            get => _containerConfiguration.GetConfiguration<bool>(AwsBlobProviderConfigurationNames.UseTemporaryCredentials);
            set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.UseTemporaryCredentials, value);
        }

        public bool UseTemporaryFederatedCredentials
        {
            get => _containerConfiguration.GetConfiguration<bool>(AwsBlobProviderConfigurationNames.UseTemporaryFederatedCredentials);
            set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.UseTemporaryFederatedCredentials, value);
        }

        public string ProfileName
        {
            get => _containerConfiguration.GetConfiguration<string>(AwsBlobProviderConfigurationNames.ProfileName);
            set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.ProfileName, value);
        }

        public string ProfilesLocation
        {
            get => _containerConfiguration.GetConfiguration<string>(AwsBlobProviderConfigurationNames.ProfilesLocation);
            set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.ProfilesLocation, value);
        }

        /// <summary>
        /// Set the validity period of the temporary access credential, the unit is s, the minimum is 900, and the maximum is 129600.
        /// </summary>
        public int DurationSeconds
        {
            get => _containerConfiguration.GetConfigurationOrDefault(AwsBlobProviderConfigurationNames.DurationSeconds, 0);
            set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.DurationSeconds, value);
        }

        public string Name
        {
            get => _containerConfiguration.GetConfiguration<string>(AwsBlobProviderConfigurationNames.Name);
            set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.Name, value);
        }

        public string Policy
        {
            get => _containerConfiguration.GetConfiguration<string>(AwsBlobProviderConfigurationNames.Policy);
            set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.Policy, value);
        }

        public RegionEndpoint Region
        {
            get => _containerConfiguration.GetConfiguration<RegionEndpoint>(AwsBlobProviderConfigurationNames.Region);
            set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.Region, Check.NotNull(value, nameof(value)));
        }

        /// <summary>
        /// This name may only contain lowercase letters, numbers, and hyphens, and must begin with a letter or a number.
        /// Each hyphen must be preceded and followed by a non-hyphen character.
        /// The name must also be between 3 and 63 characters long.
        /// If this parameter is not specified, the ContainerName of the <see cref="BlobProviderArgs"/> will be used.
        /// </summary>
        public string ContainerName
        {
            get => _containerConfiguration.GetConfiguration<string>(AwsBlobProviderConfigurationNames.ContainerName);
            set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.ContainerName, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        /// <summary>
        /// Default value: false.
        /// </summary>
        public bool CreateContainerIfNotExists
        {
            get => _containerConfiguration.GetConfigurationOrDefault(AwsBlobProviderConfigurationNames.CreateContainerIfNotExists, false);
            set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.CreateContainerIfNotExists, value);
        }

        private readonly BlobContainerConfiguration _containerConfiguration;

        public AwsBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }
    }
}
