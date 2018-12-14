using Microsoft.Extensions.Options;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.FileSystem
{
    public class AbpFileSystemStorageProvider : AbpStorageProviderBase<FileSystemParsedOptions, FileSystemProviderInstanceOptions, FileSystemStoreOptions, FileSystemScopedStoreOptions>
    {
        public const string ProviderName = "FileSystem";
        private readonly IPublicUrlProvider _publicUrlProvider;
        private readonly IAbpExtendedPropertiesProvider _extendedPropertiesProvider;

        public AbpFileSystemStorageProvider(IOptions<FileSystemParsedOptions> options, IPublicUrlProvider publicUrlProvider = null, IAbpExtendedPropertiesProvider extendedPropertiesProvider = null)
            : base(options)
        {
            _publicUrlProvider = publicUrlProvider;
            _extendedPropertiesProvider = extendedPropertiesProvider;
        }

        public override string Name => ProviderName;

        protected override IAbpStore BuildStoreInternal(string storeName, FileSystemStoreOptions storeOptions)
        {
            return new AbpFileSystemStore(
                storeOptions,
                _publicUrlProvider,
                _extendedPropertiesProvider);
        }
    }
}
