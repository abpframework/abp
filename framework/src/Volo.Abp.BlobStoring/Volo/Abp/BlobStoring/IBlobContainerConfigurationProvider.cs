namespace Volo.Abp.BlobStoring
{
    public interface IBlobContainerConfigurationProvider
    {
        BlobContainerConfiguration Get(string name);
    }
}