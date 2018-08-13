using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Storage.FileSystem.Server
{
    [DependsOn(typeof(AbpFileSystemStorageModule))]
    public class AbpFileSystemStorageServerModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpFileSystemStorageServerModule>();
        }
    }
}