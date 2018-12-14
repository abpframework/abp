namespace Volo.Abp.Storage.Configuration
{
    public class FileSystemScopedStoreOptions : FileSystemStoreOptions, IScopedStoreOptions
    {
        public string FolderNameFormat { get; set; }
    }
}
