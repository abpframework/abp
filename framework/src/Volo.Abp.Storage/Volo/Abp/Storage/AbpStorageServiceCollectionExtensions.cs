using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Storage.Configuration;
using Volo.Abp.Storage.Internal;

namespace Volo.Abp.Storage
{
    public static class AbpStorageServiceCollectionExtensions
    {
        public static IServiceCollection AddAbpStorage([NotNull] this IServiceCollection services)
        {
            Check.NotNull(services, nameof(services));
            
            services.TryAddTransient<IAbpStorageFactory, AbpStorageFactory>();
            services.TryAdd(ServiceDescriptor.Transient(typeof(IAbpStore<>), typeof(GenericStoreProxy<>)));
            return services;
        }

        public static IServiceCollection AddAbpStorage([NotNull] this IServiceCollection services,
            [NotNull] IConfigurationSection configurationSection)
        {
            Check.NotNull(services, nameof(services));
            
            Check.NotNull(configurationSection, nameof(configurationSection));
            
            return services
                .Configure<AbpStorageOptions>(configurationSection)
                .AddAbpStorage();
        }

        public static IServiceCollection AddAbpStorage([NotNull] this IServiceCollection services,
            [NotNull] IConfigurationRoot configurationRoot)
        {
            Check.NotNull(services, nameof(services));
            
            Check.NotNull(configurationRoot, nameof(configurationRoot));
            
            return services
                .Configure<AbpStorageOptions>(
                    configurationRoot.GetSection(AbpStorageOptions.DefaultConfigurationSectionName))
                .Configure<AbpStorageOptions>(storageOptions =>
                {
                    var connectionStrings = new Dictionary<string, string>();
                    configurationRoot.GetSection("ConnectionStrings").Bind(connectionStrings);

                    if (storageOptions.ConnectionStrings != null)
                        foreach (var existingConnectionString in storageOptions.ConnectionStrings)
                            connectionStrings[existingConnectionString.Key] = existingConnectionString.Value;

                    storageOptions.ConnectionStrings = connectionStrings;
                })
                .AddAbpStorage();
        }
    }
}