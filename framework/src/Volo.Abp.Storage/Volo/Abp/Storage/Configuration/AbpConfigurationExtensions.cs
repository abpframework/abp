using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Storage.Exceptions;

namespace Volo.Abp.Storage.Configuration
{
    public static class AbpConfigurationExtensions
    {
        public static IReadOnlyDictionary<string, TOptions> Parse<TOptions>(
            this IReadOnlyDictionary<string, IConfigurationSection> unparsedConfiguration)
            where TOptions : class, INamedElementOptions, new()
        {
            if (unparsedConfiguration == null) return new Dictionary<string, TOptions>();

            return unparsedConfiguration
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => BindOptions<TOptions>(kvp));
        }

        public static TStoreOptions GetStoreConfiguration<TInstanceOptions, TStoreOptions, TScopedStoreOptions>(
            this IAbpParsedOptions<TInstanceOptions, TStoreOptions, TScopedStoreOptions> parsedOptions,
            string storeName, bool throwIfNotFound = true)
            where TInstanceOptions : class, IProviderInstanceOptions
            where TStoreOptions : class, IAbpStoreOptions
            where TScopedStoreOptions : class, TStoreOptions, IScopedStoreOptions
        {
            parsedOptions.ParsedStores.TryGetValue(storeName, out var storeOptions);
            if (storeOptions != null) return storeOptions;

            if (throwIfNotFound) throw new StoreNotFoundException(storeName);

            return null;
        }

        public static TScopedStoreOptions
            GetScopedStoreConfiguration<TInstanceOptions, TStoreOptions, TScopedStoreOptions>(
                this IAbpParsedOptions<TInstanceOptions, TStoreOptions, TScopedStoreOptions> parsedOptions,
                string storeName, bool throwIfNotFound = true)
            where TInstanceOptions : class, IProviderInstanceOptions
            where TStoreOptions : class, IAbpStoreOptions
            where TScopedStoreOptions : class, TStoreOptions, IScopedStoreOptions
        {
            parsedOptions.ParsedScopedStores.TryGetValue(storeName, out var scopedStoreOptions);
            if (scopedStoreOptions != null) return scopedStoreOptions;

            if (throwIfNotFound) throw new StoreNotFoundException(storeName);

            return null;
        }

        public static void Compute<TParsedOptions, TInstanceOptions, TStoreOptions, TScopedStoreOptions>(
            this TInstanceOptions parsedProviderInstance, TParsedOptions options)
            where TParsedOptions : class, IAbpParsedOptions<TInstanceOptions, TStoreOptions, TScopedStoreOptions>
            where TInstanceOptions : class, IProviderInstanceOptions, new()
            where TStoreOptions : class, IAbpStoreOptions, new()
            where TScopedStoreOptions : class, TStoreOptions, IScopedStoreOptions
        {
            options.BindProviderInstanceOptions(parsedProviderInstance);
        }

        public static void Compute<TParsedOptions, TInstanceOptions, TStoreOptions, TScopedStoreOptions>(
            this TStoreOptions parsedStore, TParsedOptions options)
            where TParsedOptions : class, IAbpParsedOptions<TInstanceOptions, TStoreOptions, TScopedStoreOptions>
            where TInstanceOptions : class, IProviderInstanceOptions, new()
            where TStoreOptions : class, IAbpStoreOptions, new()
            where TScopedStoreOptions : class, TStoreOptions, IScopedStoreOptions
        {
            if (string.IsNullOrEmpty(parsedStore.ContainerName)) parsedStore.ContainerName = parsedStore.Name;

            TInstanceOptions instanceOptions = null;
            if (!string.IsNullOrEmpty(parsedStore.ProviderName))
            {
                options.ParsedProviderInstances.TryGetValue(parsedStore.ProviderName, out instanceOptions);
                if (instanceOptions == null) return;

                parsedStore.ProviderType = instanceOptions.Type;
            }

            options.BindStoreOptions(parsedStore, instanceOptions);
        }

        public static TStoreOptions ParseStoreOptions<TParsedOptions, TInstanceOptions, TStoreOptions,
            TScopedStoreOptions>(this IAbpStoreOptions storeOptions, TParsedOptions options)
            where TParsedOptions : class, IAbpParsedOptions<TInstanceOptions, TStoreOptions, TScopedStoreOptions>, new()
            where TInstanceOptions : class, IProviderInstanceOptions, new()
            where TStoreOptions : class, IAbpStoreOptions, new()
            where TScopedStoreOptions : class, TStoreOptions, IScopedStoreOptions
        {
            if (!(storeOptions is TStoreOptions parsedStoreOptions))
                parsedStoreOptions = new TStoreOptions
                {
                    Name = storeOptions.Name,
                    ProviderName = storeOptions.ProviderName,
                    ProviderType = storeOptions.ProviderType,
                    AccessLevel = storeOptions.AccessLevel,
                    ContainerName = storeOptions.ContainerName
                };

            parsedStoreOptions.Compute<TParsedOptions, TInstanceOptions, TStoreOptions, TScopedStoreOptions>(options);
            return parsedStoreOptions;
        }

        private static TOptions BindOptions<TOptions>(KeyValuePair<string, IConfigurationSection> kvp)
            where TOptions : class, INamedElementOptions, new()
        {
            var options = new TOptions
            {
                Name = kvp.Key
            };

            kvp.Value.Bind(options);
            return options;
        }
    }
}