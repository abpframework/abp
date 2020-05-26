namespace Volo.Abp.BlobStoring
{
    public class AbpBlobStoringOptions
    {
        public BlobContainerConfigurationDictionary Containers { get; }

        public AbpBlobStoringOptions()
        {
            Containers = new BlobContainerConfigurationDictionary();
        }
    }
}