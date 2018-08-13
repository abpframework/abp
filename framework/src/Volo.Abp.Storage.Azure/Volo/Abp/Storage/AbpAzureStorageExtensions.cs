using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Storage.Azure;
using Volo.Abp.Storage.Azure.Configuration;
using Volo.Abp.Storage.Internal;

namespace Volo.Abp.Storage
{
    public static class AbpAzureStorageExtensions
    {
        public static IServiceCollection AddAzureStorage([NotNull] this IServiceCollection services)
        {
            return services
                .AddSingleton<IConfigureOptions<AbpAzureParsedOptions>, StoreConfigureProviderOptions<
                    AbpAzureParsedOptions, AbpAzureProviderInstanceOptions, AbpAzureStoreOptions,
                    AbpAzureScopedStoreOptions>>()
                .AddAzureStorageServices();
        }

        private static IServiceCollection AddAzureStorageServices([NotNull] this IServiceCollection services)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IAbpStorageProvider, AbpAzureStorageProvider>());
            return services;
        }
    }
}