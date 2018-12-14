using System;
using Volo.Abp.Storage.FileSystem;
using Volo.Abp.Storage.FileSystem.ExtendedProperties;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpFileSystemExtendedPropertiesServiceCollectionExtensions
    {
        public static IServiceCollection AddAbpFileSystemExtendedProperties(this IServiceCollection services,
            Action<FileSystemExtendedPropertiesOptions> configure = null)
        {
            if (configure == null) configure = o => { };

            services.Configure(configure);
            services.AddTransient<IAbpExtendedPropertiesProvider, AbpExtendedPropertiesProvider>();
            return services;
        }
    }
}