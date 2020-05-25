namespace Volo.Abp.BlobStoring
{
    public class AbpBlobStoringOptions
    {
        public BlobContainerConfigurationDictionary Containers { get; set; }

        public AbpBlobStoringOptions()
        {
            Containers = new BlobContainerConfigurationDictionary();
        }
    }
}