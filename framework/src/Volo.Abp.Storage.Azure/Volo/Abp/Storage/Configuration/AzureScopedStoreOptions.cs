namespace Volo.Abp.Storage.Configuration
{
    public class AzureScopedStoreOptions : AzureStoreOptions, IScopedStoreOptions
    {
        public string FolderNameFormat { get; set; }
    }
}
