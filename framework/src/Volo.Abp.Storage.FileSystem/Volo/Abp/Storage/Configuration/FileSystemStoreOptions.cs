using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Volo.Abp.Storage.Configuration
{
    public class FileSystemStoreOptions : StoreOptions
    {
        public string RootPath { get; set; }

        public string AbsolutePath 
            => string.IsNullOrEmpty(RootPath) 
            ? FolderName : string.IsNullOrEmpty(FolderName) 
            ? RootPath : Path.Combine(RootPath, FolderName);

        public override IEnumerable<IAbpStorageOptionError> Validate(bool throwOnError = true)
        {
            var baseErrors = base.Validate(throwOnError);
            var optionErrors = new List<AbpStorageOptionError>();

            if (string.IsNullOrEmpty(AbsolutePath))
            {
                PushMissingPropertyError(optionErrors, nameof(AbsolutePath));
            }

            var finalErrors = baseErrors.Concat(optionErrors);

            if (throwOnError && finalErrors.Any())
            {
                throw new BadStoreConfigurationException(Name, finalErrors);
            }

            return finalErrors;
        }
    }
}
