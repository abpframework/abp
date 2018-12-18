using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage
{
    public class FileSystemSampleStoreOptions : IAbpStoreOptions
    {
        public FileSystemSampleStoreOptions()
        {
            Name = "SampleStore";
            ProviderType = "FileSystem";
            FolderName = Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        "SampleStore");
        }

        public string ProviderName { get; set; }

        public string ProviderType { get; set; }

        public AbpStorageAccessLevel AccessLevel { get; set; }

        public string FolderName { get; set; }

        public string Name { get; set; }

        public IEnumerable<IAbpStorageOptionError> Validate(bool throwOnError = true)
        {
            return Enumerable.Empty<IAbpStorageOptionError>();
        }
    }
}
