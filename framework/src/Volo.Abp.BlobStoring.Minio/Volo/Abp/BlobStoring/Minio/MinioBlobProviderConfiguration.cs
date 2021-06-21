namespace Volo.Abp.BlobStoring.Minio
{
    public class MinioBlobProviderConfiguration
    {
        public string BucketName
        {
            get => _containerConfiguration.GetConfigurationOrDefault<string>(MinioBlobProviderConfigurationNames.BucketName);
            set => _containerConfiguration.SetConfiguration(MinioBlobProviderConfigurationNames.BucketName, value);
        }

        /// <summary>
        /// endPoint is an URL, domain name, IPv4 address or IPv6 address.
        /// </summary>
        public string EndPoint
        {
            get => _containerConfiguration.GetConfiguration<string>(MinioBlobProviderConfigurationNames.EndPoint);
            set => _containerConfiguration.SetConfiguration(MinioBlobProviderConfigurationNames.EndPoint, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        /// <summary>
        /// accessKey is like user-id that uniquely identifies your account.This field is optional and can be omitted for anonymous access.
        /// </summary>
        public string AccessKey
        {
            get => _containerConfiguration.GetConfiguration<string>(MinioBlobProviderConfigurationNames.AccessKey);
            set => _containerConfiguration.SetConfiguration(MinioBlobProviderConfigurationNames.AccessKey, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        /// <summary>
        /// secretKey is the password to your account.This field is optional and can be omitted for anonymous access.
        /// </summary>
        public string SecretKey
        {
            get => _containerConfiguration.GetConfiguration<string>(MinioBlobProviderConfigurationNames.SecretKey);
            set => _containerConfiguration.SetConfiguration(MinioBlobProviderConfigurationNames.SecretKey, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        /// <summary>
        ///connect to  to MinIO Client object to use https instead of http
        /// </summary>
        public bool WithSSL
        {
            get => _containerConfiguration.GetConfigurationOrDefault(MinioBlobProviderConfigurationNames.WithSSL, false);
            set => _containerConfiguration.SetConfiguration(MinioBlobProviderConfigurationNames.WithSSL, value);
        }

        /// <summary>
        ///Default value: false.
        /// </summary>
        public bool CreateBucketIfNotExists
        {
            get => _containerConfiguration.GetConfigurationOrDefault(MinioBlobProviderConfigurationNames.CreateBucketIfNotExists, false);
            set => _containerConfiguration.SetConfiguration(MinioBlobProviderConfigurationNames.CreateBucketIfNotExists, value);
        }

        private readonly BlobContainerConfiguration _containerConfiguration;

        public MinioBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }
    }
}
