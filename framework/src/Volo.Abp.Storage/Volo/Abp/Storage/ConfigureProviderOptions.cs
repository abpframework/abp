using System.Linq;
using Microsoft.Extensions.Options;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage
{
    public class ConfigureProviderOptions<TParsedOptions, TInstanceOptions, TStoreOptions, TScopedStoreOptions> : IConfigureOptions<TParsedOptions>
        where TParsedOptions : class, IAbpParsedOptions<TInstanceOptions, TStoreOptions, TScopedStoreOptions>
        where TInstanceOptions : class, IProviderInstanceOptions, new()
        where TStoreOptions : class, IAbpStoreOptions, new()
        where TScopedStoreOptions : class, TStoreOptions, IScopedStoreOptions, new()
    {
        private readonly AbpStorageOptions _storageOptions;

        public ConfigureProviderOptions(IOptions<AbpStorageOptions> storageOptions)
        {
            _storageOptions = storageOptions.Value;
        }

        public void Configure(TParsedOptions options)
        {
            if (_storageOptions == null)
            {
                return;
            }

            options.ConnectionStrings = _storageOptions.ConnectionStrings;

            options.ParsedProviderInstances = _storageOptions.Providers.Parse<TInstanceOptions>()
                .Where(kvp => kvp.Value.Type == options.Name)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            foreach (var parsedProviderInstance in options.ParsedProviderInstances)
            {
                parsedProviderInstance.Value.Compute<TParsedOptions, TInstanceOptions, TStoreOptions, TScopedStoreOptions>(options);
            }

            var parsedStores = _storageOptions.Stores.Parse<TStoreOptions>();

            foreach (var parsedStore in parsedStores)
            {
                parsedStore.Value.Compute<TParsedOptions, TInstanceOptions, TStoreOptions, TScopedStoreOptions>(options);
            }

            options.ParsedStores = parsedStores
                .Where(kvp => kvp.Value.ProviderType == options.Name)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var parsedScopedStores = _storageOptions.ScopedStores.Parse<TScopedStoreOptions>();

            foreach (var parsedScopedStore in parsedScopedStores)
            {
                parsedScopedStore.Value.Compute<TParsedOptions, TInstanceOptions, TStoreOptions, TScopedStoreOptions>(options);
            }

            options.ParsedScopedStores = parsedScopedStores
                .Where(kvp => kvp.Value.ProviderType == options.Name)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
