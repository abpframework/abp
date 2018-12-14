using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Volo.Abp.Storage.Configuration
{
    public class AbpStorageOptions : IAbpParsedOptions<ProviderInstanceOptions, StoreOptions, ScopedStoreOptions>
    {
        public const string DefaultConfigurationSectionName = "AppStorage";

        private readonly Lazy<IReadOnlyDictionary<string, ProviderInstanceOptions>> _parsedProviderInstances;
        private readonly Lazy<IReadOnlyDictionary<string, StoreOptions>> _parsedStores;
        private readonly Lazy<IReadOnlyDictionary<string, ScopedStoreOptions>> _parsedScopedStores;

        public AbpStorageOptions()
        {
            _parsedProviderInstances = new Lazy<IReadOnlyDictionary<string, ProviderInstanceOptions>>(
                () => Providers.Parse<ProviderInstanceOptions>());
            
            _parsedStores = new Lazy<IReadOnlyDictionary<string, StoreOptions>>(
                () => Stores.Parse<StoreOptions>());
            
            _parsedScopedStores = new Lazy<IReadOnlyDictionary<string, ScopedStoreOptions>>(
                () => ScopedStores.Parse<ScopedStoreOptions>());
        }

        public string Name => DefaultConfigurationSectionName;

        public IReadOnlyDictionary<string, IConfigurationSection> Providers { get; set; }

        public IReadOnlyDictionary<string, IConfigurationSection> Stores { get; set; }

        public IReadOnlyDictionary<string, IConfigurationSection> ScopedStores { get; set; }

        public IReadOnlyDictionary<string, string> ConnectionStrings { get; set; }

        public IReadOnlyDictionary<string, ProviderInstanceOptions> ParsedProviderInstances
        {
            get => _parsedProviderInstances.Value;
            set { }
        }

        public IReadOnlyDictionary<string, StoreOptions> ParsedStores
        {
            get => _parsedStores.Value;
            set { }
        }

        public IReadOnlyDictionary<string, ScopedStoreOptions> ParsedScopedStores
        {
            get => _parsedScopedStores.Value;
            set { }
        }

        public void BindProviderInstanceOptions(ProviderInstanceOptions providerInstanceOptions)
        {
        }

        public void BindStoreOptions(StoreOptions storeOptions, ProviderInstanceOptions providerInstanceOptions)
        {
        }
    }
}