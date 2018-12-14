using System.Collections.Generic;

namespace Volo.Abp.Storage.Configuration
{
    public interface IAbpStoreOptions : INamedElementOptions
    {
        string ProviderName { get; set; }

        string ProviderType { get; set; }

        AbpStorageAccessLevel AccessLevel { get; set; }

        string FolderName { get; set; }

        IEnumerable<IAbpStorageOptionError> Validate(bool throwOnError = true);
    }
}
