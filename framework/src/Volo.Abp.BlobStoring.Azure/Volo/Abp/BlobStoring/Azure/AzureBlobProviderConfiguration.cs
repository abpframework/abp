namespace Volo.Abp.BlobStoring.Azure
{
    public class AzureBlobProviderConfiguration
    {
        public string ConnectionString
        {
            get => _containerConfiguration.GetConfiguration<string>(AzureBlobProviderConfigurationNames.ConnectionString);
            set => _containerConfiguration.SetConfiguration(AzureBlobProviderConfigurationNames.ConnectionString, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        public string ContainerName
        {
            get => _containerConfiguration.GetConfiguration<string>(AzureBlobProviderConfigurationNames.ContainerName);
            set => _containerConfiguration.SetConfiguration(AzureBlobProviderConfigurationNames.ContainerName, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        private readonly BlobContainerConfiguration _containerConfiguration;

        public AzureBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }
    }
}
