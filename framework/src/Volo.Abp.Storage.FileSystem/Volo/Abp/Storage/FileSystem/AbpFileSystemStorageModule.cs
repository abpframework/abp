using Volo.Abp.Modularity;

namespace Volo.Abp.Storage.FileSystem
{
    [DependsOn(typeof(AbpStorageModule))]
    public class AbpFileSystemStorageModule : AbpModule
    {
        
    }
}