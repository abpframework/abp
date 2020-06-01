using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring
{
    public static class BlobContainerConfigurationExtensions
    {
        public static T GetConfiguration<T>(
            [NotNull] this BlobContainerConfiguration containerConfiguration,
            [NotNull] string name)
        {
            return (T) containerConfiguration.GetConfiguration(name);
        }
        
        public static object GetConfiguration(
            [NotNull] this BlobContainerConfiguration containerConfiguration,
            [NotNull] string name)
        {
            var value = containerConfiguration.GetConfigurationOrNull(name);
            if (value == null)
            {
                throw new AbpException($"Could not find the configuration value for '{name}'!");
            }

            return value;
        }
    }
}