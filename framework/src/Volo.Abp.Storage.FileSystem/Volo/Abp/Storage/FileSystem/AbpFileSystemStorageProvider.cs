using Microsoft.Extensions.Options;
using Volo.Abp.Storage.FileSystem.Configuration;
using Volo.Abp.Storage.Internal;

namespace Volo.Abp.Storage.FileSystem
{
    public class AbpFileSystemStorageProvider : AbpStorageProviderBase<AbpFileSystemParsedOptions,
        AbpFileSystemProviderInstanceOptions, AbpFileSystemStoreOptions, AbpFileSystemScopedStoreOptions>
    {
        public const string ProviderName = "FileSystem";
        private readonly IExtendedPropertiesProvider _extendedPropertiesProvider;
        private readonly IPublicUrlProvider _publicUrlProvider;

        public AbpFileSystemStorageProvider(IOptions<AbpFileSystemParsedOptions> options,
            IPublicUrlProvider publicUrlProvider = null, IExtendedPropertiesProvider extendedPropertiesProvider = null)
            : base(options)
        {
            _publicUrlProvider = publicUrlProvider;
            _extendedPropertiesProvider = extendedPropertiesProvider;
        }

        public override string Name => ProviderName;

        protected override IAbpStore BuildStoreInternal(string storeName, AbpFileSystemStoreOptions storeOptions)
        {
            return new AbpFileSystemStore(
                storeOptions,
                _publicUrlProvider,
                _extendedPropertiesProvider);
        }
    }
}