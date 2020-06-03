namespace Volo.Abp.BlobStoring.Azure
{
    public class AzureBlobProviderConfiguration
    {
        private readonly BlobContainerConfiguration _containerConfiguration;

        public AzureBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }
    }
}
