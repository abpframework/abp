using System.IO;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Storage;
using Volo.Abp.Storage.Configuration;
using Volo.Abp.Storage.FileSystem;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpFileSystemStorageServiceCollectionExtensions
    {
        public static IServiceCollection AddAbpFileSystemStorage(this IServiceCollection services, string rootPath)
        {
            return services
                .Configure<FileSystemParsedOptions>(options => options.RootPath = rootPath)
                .AddFileSystemStorageServices();
        }

        public static IServiceCollection AddAbpFileSystemStorage(this IServiceCollection services)
        {
            return services
                .Configure<FileSystemParsedOptions>(options => options.RootPath = Directory.GetCurrentDirectory())
                .AddFileSystemStorageServices();
        }

        private static IServiceCollection AddFileSystemStorageServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IConfigureOptions<FileSystemParsedOptions>, ConfigureProviderOptions<
                    FileSystemParsedOptions, FileSystemProviderInstanceOptions, FileSystemStoreOptions,
                    FileSystemScopedStoreOptions>>();
            services.TryAddEnumerable(ServiceDescriptor.Transient<IAbpStorageProvider, AbpFileSystemStorageProvider>());
            return services;
        }
    }
}