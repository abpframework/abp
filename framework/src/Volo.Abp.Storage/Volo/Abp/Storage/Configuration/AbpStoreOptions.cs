using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Storage.Exceptions;

namespace Volo.Abp.Storage.Configuration
{
    public class AbpStoreOptions : IAbpStoreOptions
    {
        private const string MissingPropertyErrorMessage = "{0} should be defined.";

        public string Name { get; set; }

        public string ProviderName { get; set; }

        public string ProviderType { get; set; }

        public BlobAccessLevel AccessLevel { get; set; }

        public string ContainerName { get; set; }

        public virtual IEnumerable<IAbpOptionError> Validate(bool throwOnError = true)
        {
            var optionErrors = new List<AbpOptionError>();

            if (string.IsNullOrEmpty(Name)) PushMissingPropertyError(optionErrors, nameof(Name));

            if (string.IsNullOrEmpty(ProviderName) && string.IsNullOrEmpty(ProviderType))
                optionErrors.Add(new AbpOptionError
                {
                    PropertyName = "Provider",
                    ErrorMessage =
                        $"You should set either a {nameof(ProviderType)} or a {nameof(ProviderName)} for each Store."
                });

            if (string.IsNullOrEmpty(ContainerName)) PushMissingPropertyError(optionErrors, nameof(ContainerName));

            if (throwOnError && optionErrors.Any()) throw new BadStoreConfiguration(Name, optionErrors);

            return optionErrors;
        }

        protected void PushMissingPropertyError(List<AbpOptionError> optionErrors, string propertyName)
        {
            optionErrors.Add(new AbpOptionError
            {
                PropertyName = propertyName,
                ErrorMessage = string.Format(MissingPropertyErrorMessage, propertyName)
            });
        }
    }
}