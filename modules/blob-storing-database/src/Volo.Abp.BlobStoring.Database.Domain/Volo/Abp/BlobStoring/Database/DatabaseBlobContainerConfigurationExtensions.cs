using System;

namespace Volo.Abp.BlobStoring.Database
{
    public static class DatabaseBlobContainerConfigurationExtensions
    {
        public static DatabaseBlobProviderConfiguration GetDatabaseConfiguration(
            this BlobContainerConfiguration containerConfiguration)
        {
            return new DatabaseBlobProviderConfiguration(containerConfiguration);
        }

        public static BlobContainerConfiguration UseDatabase(
            this BlobContainerConfiguration containerConfiguration,
            Action<DatabaseBlobProviderConfiguration> fileSystemConfigureAction)
        {
            containerConfiguration.ProviderType = typeof(DatabaseBlobProvider);

            fileSystemConfigureAction(new DatabaseBlobProviderConfiguration(containerConfiguration));

            return containerConfiguration;
        }

        public static BlobContainerConfiguration UseDatabase(this BlobContainerConfiguration containerConfiguration)
        {
            containerConfiguration.ProviderType = typeof(DatabaseBlobProvider);

            return containerConfiguration;
        }
    }
}