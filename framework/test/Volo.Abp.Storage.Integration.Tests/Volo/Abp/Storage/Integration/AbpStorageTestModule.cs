using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Storage.Configuration;
using Volo.Abp.Storage.FileSystem.ExtendedProperties;

namespace Volo.Abp.Storage.Integration
{
    [DependsOn(typeof(AbpTestBaseModule))]
    [DependsOn(typeof(AbpStorageModule))]
    [DependsOn(typeof(AbpAzureStorageModule))]
    [DependsOn(typeof(AbpFileSystemStorageModule))]
    [DependsOn(typeof(AbpFileSystemExtendedPropertiesModule))]
    public class AbpStorageTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<AbpStorageOptions>(context.Services.GetConfiguration().GetSection("Storage"));
            context.Services.Configure<TestStore>(context.Services.GetConfiguration().GetSection("TestStore"));
        }
    }
}