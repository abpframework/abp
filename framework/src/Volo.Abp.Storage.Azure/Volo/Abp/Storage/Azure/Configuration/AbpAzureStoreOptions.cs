using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Storage.Configuration;
using Volo.Abp.Storage.Exceptions;

namespace Volo.Abp.Storage.Azure.Configuration
{
    public class AbpAzureStoreOptions : AbpStoreOptions
    {
        public string ConnectionString { get; set; }

        public string ConnectionStringName { get; set; }

        public override IEnumerable<IAbpOptionError> Validate(bool throwOnError = true)
        {
            var baseErrors = base.Validate(throwOnError);
            var optionErrors = new List<AbpOptionError>();

            if (string.IsNullOrEmpty(ConnectionString))
                PushMissingPropertyError(optionErrors, nameof(ConnectionString));

            var finalErrors = baseErrors.Concat(optionErrors);
            if (throwOnError && finalErrors.Any()) throw new BadStoreConfiguration(Name, finalErrors);

            return finalErrors;
        }
    }
}