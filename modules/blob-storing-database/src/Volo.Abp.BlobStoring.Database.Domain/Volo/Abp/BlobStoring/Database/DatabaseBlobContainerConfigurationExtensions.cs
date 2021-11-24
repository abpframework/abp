namespace Volo.Abp.BlobStoring.Database;

public static class DatabaseBlobContainerConfigurationExtensions
{
    public static BlobContainerConfiguration UseDatabase(
        this BlobContainerConfiguration containerConfiguration)
    {
        containerConfiguration.ProviderType = typeof(DatabaseBlobProvider);
        return containerConfiguration;
    }
}
