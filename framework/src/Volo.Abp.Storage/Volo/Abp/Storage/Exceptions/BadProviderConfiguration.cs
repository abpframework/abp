using System;

namespace Volo.Abp.Storage.Exceptions
{
    public class BadProviderConfiguration : Exception
    {
        public BadProviderConfiguration(string providerName)
            : base($"The provider '{providerName}' was not properly configured.")
        {
        }

        public BadProviderConfiguration(string providerName, string details)
            : base($"The providerName '{providerName}' was not properly configured. {details}")
        {
        }
    }
}