namespace Volo.Abp.BlobStoring.Memory;

public static class MemoryBlobContainerConfigurationExtensions
{
    public static BlobContainerConfiguration UseMemory(this BlobContainerConfiguration containerConfiguration)
    {
        containerConfiguration.ProviderType = typeof(MemoryBlobProvider);
        return containerConfiguration;
    }
}
