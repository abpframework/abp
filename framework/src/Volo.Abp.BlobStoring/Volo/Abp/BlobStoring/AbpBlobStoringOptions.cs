namespace Volo.Abp.BlobStoring
{
    public class AbpBlobStoringOptions
    {
        public BlobContainerConfigurations Containers { get; }

        public AbpBlobStoringOptions()
        {
            Containers = new BlobContainerConfigurations();
        }
    }
}