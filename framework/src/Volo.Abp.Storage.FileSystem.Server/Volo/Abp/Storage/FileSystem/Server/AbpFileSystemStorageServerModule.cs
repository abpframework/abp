using Volo.Abp.Modularity;

namespace Volo.Abp.Storage.FileSystem.Server
{
    [DependsOn(typeof(AbpFileSystemStorageModule))]
    public class AbpFileSystemStorageServerModule : AbpModule
    {

    }
}