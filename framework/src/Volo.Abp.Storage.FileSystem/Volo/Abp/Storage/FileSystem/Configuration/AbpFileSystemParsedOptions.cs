using System.Collections.Generic;
using System.IO;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.FileSystem.Configuration
{
    public class AbpFileSystemParsedOptions : IAbpParsedOptions<AbpFileSystemProviderInstanceOptions, AbpFileSystemStoreOptions,
        AbpFileSystemScopedStoreOptions>
    {
        public string RootPath { get; set; }
        public string Name => AbpFileSystemStorageProvider.ProviderName;

        public IReadOnlyDictionary<string, string> ConnectionStrings { get; set; }

        public IReadOnlyDictionary<string, AbpFileSystemProviderInstanceOptions> ParsedProviderInstances { get; set; }

        public IReadOnlyDictionary<string, AbpFileSystemStoreOptions> ParsedStores { get; set; }

        public IReadOnlyDictionary<string, AbpFileSystemScopedStoreOptions> ParsedScopedStores { get; set; }

        public void BindProviderInstanceOptions(AbpFileSystemProviderInstanceOptions providerInstanceOptions)
        {
            if (string.IsNullOrEmpty(providerInstanceOptions.RootPath))
            {
                providerInstanceOptions.RootPath = RootPath;
            }
            else
            {
                if (!Path.IsPathRooted(providerInstanceOptions.RootPath))
                    providerInstanceOptions.RootPath = Path.Combine(RootPath, providerInstanceOptions.RootPath);
            }
        }

        public void BindStoreOptions(AbpFileSystemStoreOptions storeOptions,
            AbpFileSystemProviderInstanceOptions providerInstanceOptions = null)
        {
            if (!string.IsNullOrEmpty(storeOptions.RootPath)) 
                return;
            
            if (providerInstanceOptions != null
                && storeOptions.ProviderName == providerInstanceOptions.Name)
                storeOptions.RootPath = providerInstanceOptions.RootPath;
            else
                storeOptions.RootPath = RootPath;
        }
    }
}