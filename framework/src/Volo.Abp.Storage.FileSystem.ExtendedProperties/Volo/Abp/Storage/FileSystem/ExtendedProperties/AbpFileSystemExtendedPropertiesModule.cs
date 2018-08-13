using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Storage.FileSystem.ExtendedProperties
{
    [DependsOn(typeof(AbpFileSystemStorageModule))]
    public class AbpFileSystemExtendedPropertiesModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpFileSystemExtendedPropertiesModule>();
        }
    }
}