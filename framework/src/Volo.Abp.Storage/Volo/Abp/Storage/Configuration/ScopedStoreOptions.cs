namespace Volo.Abp.Storage.Configuration
{
    public class ScopedStoreOptions : AbpStoreOptions, IScopedStoreOptions
    {
        public string FolderNameFormat { get; set; }
    }
}