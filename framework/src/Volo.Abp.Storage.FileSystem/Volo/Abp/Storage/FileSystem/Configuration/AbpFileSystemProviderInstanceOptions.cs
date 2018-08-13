using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.FileSystem.Configuration
{
    public class AbpFileSystemProviderInstanceOptions : ProviderInstanceOptions
    {
        public string RootPath { get; set; }
    }
}