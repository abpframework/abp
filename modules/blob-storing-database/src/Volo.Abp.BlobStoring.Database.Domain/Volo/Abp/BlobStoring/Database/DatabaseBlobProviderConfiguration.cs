namespace Volo.Abp.BlobStoring.Database
{
    public class DatabaseBlobProviderConfiguration
    {
        private readonly BlobContainerConfiguration _containerConfiguration;

        public DatabaseBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }
    }
}