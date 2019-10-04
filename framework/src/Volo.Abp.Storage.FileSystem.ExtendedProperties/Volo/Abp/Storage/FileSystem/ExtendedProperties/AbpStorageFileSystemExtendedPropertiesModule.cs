using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace Volo.Abp.Storage.FileSystem.ExtendedProperties
{
    [DependsOn(typeof(AbpFileSystemStorageModule),
        typeof(AbpJsonModule)
        )]
    public class AbpFileSystemStorageExtendedPropertiesModule : AbpModule
    {

    }
}