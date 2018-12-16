using Microsoft.Extensions.Logging;
using Volo.Abp.Logging;

namespace Volo.Abp.Storage
{
    public class BadProviderConfigurationException : AbpException, IHasLogLevel
    {
        public BadProviderConfigurationException(string providerName)
            : base($"The provider '{providerName}' was not properly configured.")
        {
            LogLevel = LogLevel.Warning;
        }

        public BadProviderConfigurationException(string providerName, string details)
            : base($"The providerName '{providerName}' was not properly configured. {details}")
        {
            LogLevel = LogLevel.Warning;
        }

        public LogLevel LogLevel { get; set; }
    }
}