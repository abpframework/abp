using Microsoft.Extensions.Options;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.Azure
{
    public class AbpAzureStorageProvider : AbpStorageProviderBase<AzureParsedOptions, AzureProviderInstanceOptions,
        AzureStoreOptions, AzureScopedStoreOptions>
    {
        public const string ProviderName = "Azure";

        public AbpAzureStorageProvider(IOptions<AzureParsedOptions> options)
            : base(options)
        {
        }

        public override string Name => ProviderName;

        protected override IAbpStore BuildStoreInternal(string storeName, AzureStoreOptions storeOptions)
        {
            return new AbpAzureStore(storeOptions);
        }
    }
}