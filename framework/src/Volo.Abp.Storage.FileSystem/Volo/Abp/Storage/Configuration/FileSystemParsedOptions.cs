using System.Collections.Generic;
using System.IO;
using Volo.Abp.Storage.FileSystem;

namespace Volo.Abp.Storage.Configuration
{
    public class FileSystemParsedOptions : IAbpParsedOptions<FileSystemProviderInstanceOptions, FileSystemStoreOptions, FileSystemScopedStoreOptions>
    {
        public string Name => AbpFileSystemStorageProvider.ProviderName;

        public IReadOnlyDictionary<string, string> ConnectionStrings { get; set; }

        public IReadOnlyDictionary<string, FileSystemProviderInstanceOptions> ParsedProviderInstances { get; set; }

        public IReadOnlyDictionary<string, FileSystemStoreOptions> ParsedStores { get; set; }

        public IReadOnlyDictionary<string, FileSystemScopedStoreOptions> ParsedScopedStores { get; set; }

        public string RootPath { get; set; }

        public void BindProviderInstanceOptions(FileSystemProviderInstanceOptions providerInstanceOptions)
        {
            if (string.IsNullOrEmpty(providerInstanceOptions.RootPath))
            {
                providerInstanceOptions.RootPath = RootPath;
            }
            else
            {
                if (!Path.IsPathRooted(providerInstanceOptions.RootPath))
                {
                    providerInstanceOptions.RootPath = Path.Combine(RootPath, providerInstanceOptions.RootPath);
                }
            }
        }

        public void BindStoreOptions(FileSystemStoreOptions storeOptions, FileSystemProviderInstanceOptions providerInstanceOptions = null)
        {
            if (!string.IsNullOrEmpty(storeOptions.RootPath)) return;
            
            if (providerInstanceOptions != null
                && storeOptions.ProviderName == providerInstanceOptions.Name)
            {
                storeOptions.RootPath = providerInstanceOptions.RootPath;
            }
            else
            {
                storeOptions.RootPath = RootPath;
            }
        }
    }
}
