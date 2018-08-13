using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Storage
{
    [DependsOn(typeof(AbpStorageModule))]
    public class AbpFileSystemStorageModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpFileSystemStorageModule>();
        }
    }
}