namespace Volo.Abp.BlobStoring.FileSystem
{
    public class FileSystemBlobProviderConfiguration
    {
        
        public string BasePath
        {
            get => _containerConfiguration.GetConfiguration<string>(FileSystemBlobProviderConfigurationNames.BasePath);
            set => _containerConfiguration.SetConfiguration(FileSystemBlobProviderConfigurationNames.BasePath, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        private readonly BlobContainerConfiguration _containerConfiguration;

        public FileSystemBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }
    }
}