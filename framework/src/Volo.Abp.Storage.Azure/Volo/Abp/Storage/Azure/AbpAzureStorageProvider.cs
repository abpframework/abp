using Microsoft.Extensions.Options;
using Volo.Abp.Storage.Azure.Configuration;
using Volo.Abp.Storage.Internal;

namespace Volo.Abp.Storage.Azure
{
    public class AbpAzureStorageProvider : AbpStorageProviderBase<AbpAzureParsedOptions, AbpAzureProviderInstanceOptions
        , AbpAzureStoreOptions, AbpAzureScopedStoreOptions>
    {
        public const string ProviderName = "Azure";

        public AbpAzureStorageProvider(IOptions<AbpAzureParsedOptions> options)
            : base(options)
        {
        }

        public override string Name => ProviderName;

        protected override IAbpStore BuildStoreInternal(string storeName, AbpAzureStoreOptions storeOptions)
        {
            return new AbpAzureStore(storeOptions);
        }
    }
}