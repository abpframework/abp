using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.FileSystem.Configuration
{
    public class AbpFileSystemScopedStoreOptions : AbpFileSystemStoreOptions, IScopedStoreOptions
    {
        public string FolderNameFormat { get; set; }
    }
}