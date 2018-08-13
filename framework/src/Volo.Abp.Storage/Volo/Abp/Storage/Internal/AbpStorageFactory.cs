using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Volo.Abp.Storage.Configuration;
using Volo.Abp.Storage.Exceptions;

namespace Volo.Abp.Storage.Internal
{
    public class AbpStorageFactory : IAbpStorageFactory
    {
        private readonly AbpStorageOptions _options;
        private readonly IReadOnlyDictionary<string, IAbpStorageProvider> _storageProviders;

        public AbpStorageFactory(IEnumerable<IAbpStorageProvider> storageProviders, IOptions<AbpStorageOptions> options)
        {
            _storageProviders = storageProviders.ToDictionary(sp => sp.Name, sp => sp);
            _options = options.Value;
        }

        public IAbpStore GetStore(string storeName, IAbpStoreOptions configuration)
        {
            return GetProvider(configuration).BuildStore(storeName, configuration);
        }

        public IAbpStore GetStore(string storeName)
        {
            return GetProvider(_options.GetStoreConfiguration(storeName)).BuildStore(storeName);
        }

        public IAbpStore GetScopedStore(string storeName, params object[] args)
        {
            return GetProvider(_options.GetScopedStoreConfiguration(storeName)).BuildScopedStore(storeName, args);
        }

        public bool TryGetStore(string storeName, out IAbpStore store)
        {
            var configuration = _options.GetStoreConfiguration(storeName, false);
            if (configuration != null)
            {
                var provider = GetProvider(configuration, false);
                if (provider != null)
                {
                    store = provider.BuildStore(storeName);
                    return true;
                }
            }

            store = null;
            return false;
        }

        public bool TryGetStore(string storeName, out IAbpStore store, string providerName)
        {
            var configuration = _options.GetStoreConfiguration(storeName, false);
            if (configuration != null)
            {
                var provider = GetProvider(configuration, false);
                if (provider != null && provider.Name == providerName)
                {
                    store = provider.BuildStore(storeName);
                    return true;
                }
            }

            store = null;
            return false;
        }

        private IAbpStorageProvider GetProvider(IAbpStoreOptions configuration, bool throwIfNotFound = true)
        {
            string providerTypeName = null;
            if (!string.IsNullOrEmpty(configuration.ProviderType))
            {
                providerTypeName = configuration.ProviderType;
            }
            else if (!string.IsNullOrEmpty(configuration.ProviderName))
            {
                _options.ParsedProviderInstances.TryGetValue(configuration.ProviderName,
                    out var providerInstanceOptions);
                if (providerInstanceOptions != null)
                    providerTypeName = providerInstanceOptions.Type;
                else if (throwIfNotFound)
                    throw new BadProviderConfiguration(configuration.ProviderName,
                        "Unable to find it in the configuration.");
            }
            else if (throwIfNotFound)
            {
                throw new BadStoreConfiguration(configuration.Name,
                    "You have to set either 'ProviderType' or 'ProviderName' on Store configuration.");
            }

            if (string.IsNullOrEmpty(providerTypeName)) return null;

            _storageProviders.TryGetValue(providerTypeName, out var provider);
            if (provider == null && throwIfNotFound) throw new ProviderNotFoundException(providerTypeName);

            return provider;
        }
    }
}