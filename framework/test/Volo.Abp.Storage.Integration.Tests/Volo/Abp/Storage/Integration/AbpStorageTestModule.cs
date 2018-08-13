using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
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
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                
            context.Services.AddAbpStorage(context.Services.GetConfiguration())
                .AddAzureStorage()
                .AddFileSystemStorage(Path.Combine(basePath, "FileVault"))
                .AddFileSystemExtendedProperties();
                
            context.Services.Configure<AbpStorageOptions>(context.Services.GetConfiguration().GetSection("Storage"));
            context.Services.Configure<TestStore>(context.Services.GetConfiguration().GetSection("TestStore"));
        }
    }
}