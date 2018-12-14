using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Storage;
using Volo.Abp.Storage.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpStorageServiceCollectionExtensions
    {
        public static IServiceCollection AddAbpStorage(this IServiceCollection services)
        {
            services.TryAddTransient<IAbpStorageFactory, AbpStorageFactory>();
            services.TryAdd(ServiceDescriptor.Transient(typeof(IAbpStoreWithOption<>), typeof(GenericStoreProxy<>)));
            return services;
        }

        public static IServiceCollection AddAbpStorage(this IServiceCollection services,
            IConfigurationSection configurationSection)
        {
            return services
                .Configure<AbpStorageOptions>(configurationSection)
                .AddAbpStorage();
        }

        public static IServiceCollection AddAbpStorage(this IServiceCollection services,
            IConfigurationRoot configurationRoot)
        {
            return services
                .Configure<AbpStorageOptions>(
                    configurationRoot.GetSection(AbpStorageOptions.DefaultConfigurationSectionName))
                .Configure<AbpStorageOptions>(storageOptions =>
                {
                    var connectionStrings = new Dictionary<string, string>();
                    configurationRoot.GetSection("connectionStrings").Bind(connectionStrings);

                    if (storageOptions.ConnectionStrings != null)
                        foreach (var existingConnectionString in storageOptions.ConnectionStrings)
                            connectionStrings[existingConnectionString.Key] = existingConnectionString.Value;

                    storageOptions.ConnectionStrings = connectionStrings;
                })
                .AddAbpStorage();
        }
    }
}