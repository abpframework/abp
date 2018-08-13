using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Storage
{
    [DependsOn(typeof(AbpStorageModule))]
    public class AbpAzureStorageModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpAzureStorageModule>();
        }
    }
}