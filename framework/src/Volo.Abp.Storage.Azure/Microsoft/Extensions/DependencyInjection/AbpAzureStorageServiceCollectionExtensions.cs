using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Storage;
using Volo.Abp.Storage.Azure;
using Volo.Abp.Storage.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpAzureStorageServiceCollectionExtensions
    {
        public static IServiceCollection AddAbpAzureStorage(this IServiceCollection services)
        {
            return services
                .AddSingleton<IConfigureOptions<AzureParsedOptions>, ConfigureProviderOptions<
                    AzureParsedOptions, AzureProviderInstanceOptions, AzureStoreOptions,
                    AzureScopedStoreOptions>>()
                .AddAzureStorageServices();
        }

        private static IServiceCollection AddAzureStorageServices(this IServiceCollection services)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IAbpStorageProvider, AbpAzureStorageProvider>());
            return services;
        }
    }
}