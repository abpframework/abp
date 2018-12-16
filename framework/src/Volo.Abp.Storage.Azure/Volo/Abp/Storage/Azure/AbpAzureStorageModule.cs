using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.Azure
{
    [DependsOn(typeof(AbpStorageModule))]
    public class AbpAzureStorageModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<IConfigureOptions<AzureParsedOptions>, ConfigureProviderOptions<
                    AzureParsedOptions, AzureProviderInstanceOptions, AzureStoreOptions,
                    AzureScopedStoreOptions>>();

            context.Services.TryAddEnumerable(ServiceDescriptor.Transient<IAbpStorageProvider, AbpAzureStorageProvider>());
        }
    }
}