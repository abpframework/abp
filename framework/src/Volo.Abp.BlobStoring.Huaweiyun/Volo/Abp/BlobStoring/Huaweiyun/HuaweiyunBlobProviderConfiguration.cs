namespace Volo.Abp.BlobStoring.Huaweiyun
{
    public class HuaweiyunBlobProviderConfiguration
    {

        public string AccessKeyId
        {
            get => _containerConfiguration.GetConfiguration<string>(HuaweiyunBlobProviderConfigurationNames.AccessKeyId);
            set => _containerConfiguration.SetConfiguration(HuaweiyunBlobProviderConfigurationNames.AccessKeyId, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        public string SecretAccessKey
        {
            get => _containerConfiguration.GetConfiguration<string>(HuaweiyunBlobProviderConfigurationNames.SecretAccessKey);
            set => _containerConfiguration.SetConfiguration(HuaweiyunBlobProviderConfigurationNames.SecretAccessKey, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        public string Endpoint
        {
            get => _containerConfiguration.GetConfiguration<string>(HuaweiyunBlobProviderConfigurationNames.Endpoint);
            set => _containerConfiguration.SetConfiguration(HuaweiyunBlobProviderConfigurationNames.Endpoint, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }


        /// <summary>
        /// This name may only contain lowercase letters, numbers, and hyphens, and must begin with a letter or a number.
        /// Each hyphen must be preceded and followed by a non-hyphen character.
        /// The name must also be between 3 and 63 characters long.
        /// If this parameter is not specified, the ContainerName of the <see cref="BlobProviderArgs"/> will be used.
        /// </summary>
        public string ContainerName
        {
            get => _containerConfiguration.GetConfiguration<string>(HuaweiyunBlobProviderConfigurationNames.ContainerName);
            set => _containerConfiguration.SetConfiguration(HuaweiyunBlobProviderConfigurationNames.ContainerName, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        /// <summary>
        /// Default value: false.
        /// </summary>
        public bool CreateContainerIfNotExists
        {
            get => _containerConfiguration.GetConfigurationOrDefault(HuaweiyunBlobProviderConfigurationNames.CreateContainerIfNotExists, false);
            set => _containerConfiguration.SetConfiguration(HuaweiyunBlobProviderConfigurationNames.CreateContainerIfNotExists, value);
        }

        public string ContainerLocation
        {

            get => _containerConfiguration.GetConfiguration<string>(HuaweiyunBlobProviderConfigurationNames.ContainerLocation);
            set => _containerConfiguration.SetConfiguration(HuaweiyunBlobProviderConfigurationNames.ContainerLocation, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        private readonly BlobContainerConfiguration _containerConfiguration;

        public HuaweiyunBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }
    }
}