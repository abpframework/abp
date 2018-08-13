using System;
using Microsoft.Extensions.Options;
using Volo.Abp.Storage.Configuration;
using Volo.Abp.Storage.Exceptions;

namespace Volo.Abp.Storage.Internal
{
    public abstract class
        AbpStorageProviderBase<TParsedOptions, TInstanceOptions, TStoreOptions,
            TScopedStoreOptions> : IAbpStorageProvider
        where TParsedOptions : class, IAbpParsedOptions<TInstanceOptions, TStoreOptions, TScopedStoreOptions>, new()
        where TInstanceOptions : class, IProviderInstanceOptions, new()
        where TStoreOptions : class, IAbpStoreOptions, new()
        where TScopedStoreOptions : class, TStoreOptions, IScopedStoreOptions
    {
        protected readonly TParsedOptions Options;

        public AbpStorageProviderBase(IOptions<TParsedOptions> options)
        {
            Options = options.Value;
        }

        public abstract string Name { get; }

        public IAbpStore BuildStore(string storeName)
        {
            return BuildStoreInternal(storeName, Options.GetStoreConfiguration(storeName));
        }

        public IAbpStore BuildStore(string storeName, IAbpStoreOptions storeOptions)
        {
            return BuildStoreInternal(
                storeName,
                storeOptions.ParseStoreOptions<TParsedOptions, TInstanceOptions, TStoreOptions, TScopedStoreOptions>(
                    Options));
        }

        public IAbpStore BuildScopedStore(string storeName, params object[] args)
        {
            var scopedStoreOptions = Options.GetScopedStoreConfiguration(storeName);

            try
            {
                scopedStoreOptions.ContainerName = string.Format(scopedStoreOptions.FolderNameFormat, args);
            }
            catch (Exception ex)
            {
                throw new BadScopedStoreConfiguration(storeName,
                    "Cannot format folder name. See InnerException for details.", ex);
            }

            return BuildStoreInternal(storeName,
                scopedStoreOptions
                    .ParseStoreOptions<TParsedOptions, TInstanceOptions, TStoreOptions, TScopedStoreOptions>(Options));
        }

        protected abstract IAbpStore BuildStoreInternal(string storeName, TStoreOptions storeOptions);
    }
}