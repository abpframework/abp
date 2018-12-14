using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Storage.Configuration
{
    public class AzureStoreOptions : StoreOptions
    {
        public string ConnectionString { get; set; }

        public string ConnectionStringName { get; set; }

        public override IEnumerable<IAbpStorageOptionError> Validate(bool throwOnError = true)
        {
            var baseErrors = base.Validate(throwOnError);
            var optionErrors = new List<AbpStorageOptionError>();

            if (string.IsNullOrEmpty(ConnectionString))
            {
                PushMissingPropertyError(optionErrors, nameof(ConnectionString));
            }

            var finalErrors = baseErrors.Concat(optionErrors);
            if (throwOnError && finalErrors.Any())
            {
                throw new Exceptions.BadStoreConfiguration(Name, finalErrors);
            }

            return finalErrors;
        }
    }
}
