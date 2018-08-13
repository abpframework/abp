using System.Collections.Generic;
using System.IO;
using System.Linq;
using Volo.Abp.Storage.Configuration;
using Volo.Abp.Storage.Exceptions;

namespace Volo.Abp.Storage.FileSystem.Configuration
{
    public class AbpFileSystemStoreOptions : AbpStoreOptions
    {
        public string RootPath { get; set; }

        public string AbsolutePath
        {
            get
            {
                if (string.IsNullOrEmpty(RootPath)) return ContainerName;

                return string.IsNullOrEmpty(ContainerName) 
                    ? RootPath 
                    : Path.Combine(RootPath, ContainerName);
            }
        }

        public override IEnumerable<IAbpOptionError> Validate(bool throwOnError = true)
        {
            var baseErrors = base.Validate(throwOnError);
            var optionErrors = new List<AbpOptionError>();

            if (string.IsNullOrEmpty(AbsolutePath)) PushMissingPropertyError(optionErrors, nameof(AbsolutePath));

            var finalErrors = baseErrors.Concat(optionErrors);
            if (throwOnError && finalErrors.Any()) throw new BadStoreConfiguration(Name, finalErrors);

            return finalErrors;
        }
    }
}