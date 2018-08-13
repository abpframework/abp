using System.Collections.Generic;
using Volo.Abp.Storage.Configuration;
using Volo.Abp.Storage.Exceptions;

namespace Volo.Abp.Storage.Azure.Configuration
{
    public class AbpAzureParsedOptions : IAbpParsedOptions<AbpAzureProviderInstanceOptions, AbpAzureStoreOptions,
        AbpAzureScopedStoreOptions>
    {
        public string Name => AbpAzureStorageProvider.ProviderName;

        public IReadOnlyDictionary<string, string> ConnectionStrings { get; set; }

        public IReadOnlyDictionary<string, AbpAzureProviderInstanceOptions> ParsedProviderInstances { get; set; }

        public IReadOnlyDictionary<string, AbpAzureStoreOptions> ParsedStores { get; set; }

        public IReadOnlyDictionary<string, AbpAzureScopedStoreOptions> ParsedScopedStores { get; set; }

        public void BindProviderInstanceOptions(AbpAzureProviderInstanceOptions providerInstanceOptions)
        {
            if (string.IsNullOrEmpty(providerInstanceOptions.ConnectionStringName) ||
                !string.IsNullOrEmpty(providerInstanceOptions.ConnectionString)) return;
            if (!ConnectionStrings.ContainsKey(providerInstanceOptions.ConnectionStringName))
                throw new BadProviderConfiguration(
                    providerInstanceOptions.Name,
                    $"The ConnectionString '{providerInstanceOptions.ConnectionStringName}' cannot be found. Did you call AddStorage with the ConfigurationRoot?");

            providerInstanceOptions.ConnectionString = ConnectionStrings[providerInstanceOptions.ConnectionStringName];
        }

        public void BindStoreOptions(AbpAzureStoreOptions storeOptions,
            AbpAzureProviderInstanceOptions providerInstanceOptions = null)
        {
            storeOptions.ContainerName = storeOptions.ContainerName.ToLowerInvariant();

            if (!string.IsNullOrEmpty(storeOptions.ConnectionStringName)
                && string.IsNullOrEmpty(storeOptions.ConnectionString))
            {
                if (!ConnectionStrings.ContainsKey(storeOptions.ConnectionStringName))
                    throw new BadStoreConfiguration(
                        storeOptions.Name,
                        $"The ConnectionString '{storeOptions.ConnectionStringName}' cannot be found. Did you call AddStorage with the ConfigurationRoot?");

                storeOptions.ConnectionString = ConnectionStrings[storeOptions.ConnectionStringName];
            }

            if (providerInstanceOptions == null
                || storeOptions.ProviderName != providerInstanceOptions.Name)
                return;

            if (string.IsNullOrEmpty(storeOptions.ConnectionString))
                storeOptions.ConnectionString = providerInstanceOptions.ConnectionString;
        }
    }
}