using System.Collections.Generic;

namespace Volo.Abp.Storage.Configuration
{
    public interface IAbpParsedOptions<TInstanceOptions, TStoreOptions, TScopedStoreOptions>
        where TInstanceOptions : class, IProviderInstanceOptions
        where TStoreOptions : class, IAbpStoreOptions
        where TScopedStoreOptions : class, TStoreOptions, IScopedStoreOptions
    {
        string Name { get; }

        IReadOnlyDictionary<string, string> ConnectionStrings { get; set; }

        IReadOnlyDictionary<string, TInstanceOptions> ParsedProviderInstances { get; set; }

        IReadOnlyDictionary<string, TStoreOptions> ParsedStores { get; set; }

        IReadOnlyDictionary<string, TScopedStoreOptions> ParsedScopedStores { get; set; }

        void BindProviderInstanceOptions(TInstanceOptions providerInstanceOptions);

        void BindStoreOptions(TStoreOptions storeOptions, TInstanceOptions providerInstanceOptions = null);
    }
}