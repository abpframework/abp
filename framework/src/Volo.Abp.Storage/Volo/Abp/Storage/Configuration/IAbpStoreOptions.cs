using System.Collections.Generic;

namespace Volo.Abp.Storage.Configuration
{
    public interface IAbpStoreOptions : INamedElementOptions
    {
        string ProviderName { get; set; }

        string ProviderType { get; set; }

        BlobAccessLevel AccessLevel { get; set; }

        string ContainerName { get; set; }

        IEnumerable<IAbpOptionError> Validate(bool throwOnError = true);
    }
}