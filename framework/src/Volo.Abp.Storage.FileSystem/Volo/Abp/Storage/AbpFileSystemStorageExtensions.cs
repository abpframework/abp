using System.IO;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Storage.FileSystem;
using Volo.Abp.Storage.FileSystem.Configuration;
using Volo.Abp.Storage.Internal;

namespace Volo.Abp.Storage
{
    public static class AbpFileSystemStorageExtensions
    {
        public static IServiceCollection AddFileSystemStorage([NotNull]this IServiceCollection services, string rootPath)
        {
            return services
                .Configure<AbpFileSystemParsedOptions>(options => options.RootPath = rootPath)
                .AddFileSystemStorageServices();
        }

        public static IServiceCollection AddFileSystemStorage([NotNull] this IServiceCollection services)
        {
            return services
                .Configure<AbpFileSystemParsedOptions>(options => options.RootPath = Directory.GetCurrentDirectory())
                .AddFileSystemStorageServices();
        }

        private static IServiceCollection AddFileSystemStorageServices([NotNull] this IServiceCollection services)
        {
            services
                .AddSingleton<IConfigureOptions<AbpFileSystemParsedOptions>, StoreConfigureProviderOptions<
                    AbpFileSystemParsedOptions, AbpFileSystemProviderInstanceOptions, AbpFileSystemStoreOptions,
                    AbpFileSystemScopedStoreOptions>>();
            services.TryAddEnumerable(ServiceDescriptor.Transient<IAbpStorageProvider, AbpFileSystemStorageProvider>());
            return services;
        }
    }
}