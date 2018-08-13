using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.Integration
{
    public class TestStore : IAbpStoreOptions
    {
        public TestStore()
        {
            Name = "TestStore";
            ProviderType = "FileSystem";
        }

        public string ProviderName { get; set; }

        public string ProviderType { get; set; }
        
        public BlobAccessLevel AccessLevel { get; set; }
        
        public string ContainerName { get; set; }

        public string Name { get; set; }

        public IEnumerable<IAbpOptionError> Validate(bool throwOnError = true)
        {
            return Enumerable.Empty<IAbpOptionError>();
        }
    }
}
