using Microsoft.Extensions.Logging;
using System;
using Volo.Abp.Logging;

namespace Volo.Abp.Storage
{
    public class BadScopedStoreConfigurationException : AbpException, IHasLogLevel
    {
        public BadScopedStoreConfigurationException(string storeName)
            : base($"The scoped store '{storeName}' was not properly configured.")
        {
            LogLevel = LogLevel.Warning;
        }

        public BadScopedStoreConfigurationException(string storeName, string details)
            : base($"The scoped store '{storeName}' was not properly configured. {details}")
        {
            LogLevel = LogLevel.Warning;
        }

        public BadScopedStoreConfigurationException(string storeName, string details, Exception innerException)
            : base($"The scoped store '{storeName}' was not properly configured. {details}", innerException)
        {
            LogLevel = LogLevel.Warning;
        }

        public LogLevel LogLevel { get; set; }
    }
}