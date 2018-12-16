using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Storage.Configuration
{
    public class StoreOptions : IAbpStoreOptions
    {
        private const string MissingPropertyErrorMessage = "{0} should be defined.";

        public string Name { get; set; }

        public string ProviderName { get; set; }

        public string ProviderType { get; set; }

        public AbpStorageAccessLevel AccessLevel { get; set; }

        public string FolderName { get; set; }

        public virtual IEnumerable<IAbpStorageOptionError> Validate(bool throwOnError = true)
        {
            var optionErrors = new List<AbpStorageOptionError>();

            if (string.IsNullOrEmpty(Name))
            {
                PushMissingPropertyError(optionErrors, nameof(Name));
            }

            if (string.IsNullOrEmpty(ProviderName) && string.IsNullOrEmpty(ProviderType))
            {
                optionErrors.Add(new AbpStorageOptionError
                {
                    PropertyName = "Provider",
                    ErrorMessage = $"You should set either a {nameof(ProviderType)} or a {nameof(ProviderName)} for each Store."
                });
            }

            if (string.IsNullOrEmpty(FolderName))
            {
                PushMissingPropertyError(optionErrors, nameof(FolderName));
            }

            if (throwOnError && optionErrors.Any())
            {
                throw new BadStoreConfigurationException(Name, optionErrors);
            }

            return optionErrors;
        }

        protected void PushMissingPropertyError(List<AbpStorageOptionError> optionErrors, string propertyName)
        {
            optionErrors.Add(new AbpStorageOptionError
            {
                PropertyName = propertyName,
                ErrorMessage = string.Format(MissingPropertyErrorMessage, propertyName)
            });
        }
    }
}
