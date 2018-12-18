using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.IO;
using Volo.Abp.Modularity;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.FileSystem
{
    [DependsOn(typeof(AbpStorageModule))]
    public class AbpFileSystemStorageModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<FileSystemParsedOptions>(options => options.RootPath = Directory.GetCurrentDirectory());

            context.Services.AddSingleton<IConfigureOptions<FileSystemParsedOptions>, ConfigureProviderOptions<
                    FileSystemParsedOptions, FileSystemProviderInstanceOptions, FileSystemStoreOptions,
                    FileSystemScopedStoreOptions>>();

            context.Services.TryAddEnumerable(ServiceDescriptor.Transient<IAbpStorageProvider, AbpFileSystemStorageProvider>());
        }
    }
}