using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.Azure.Configuration
{
    public class AbpAzureScopedStoreOptions : AbpAzureStoreOptions, IScopedStoreOptions
    {
        public string FolderNameFormat { get; set; }
    }
}