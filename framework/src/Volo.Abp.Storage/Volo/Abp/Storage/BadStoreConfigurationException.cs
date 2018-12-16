using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Logging;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage
{
    public class BadStoreConfigurationException : AbpException, IHasLogLevel
    {
        public BadStoreConfigurationException(string storeName)
            : base($"The store '{storeName}' was not properly configured.")
        {
            LogLevel = LogLevel.Warning;
        }

        public BadStoreConfigurationException(string storeName, string details)
            : base($"The store '{storeName}' was not properly configured. {details}")
        {
            LogLevel = LogLevel.Warning;
        }

        public BadStoreConfigurationException(string storeName, IEnumerable<IAbpStorageOptionError> errors)
            : this(storeName, string.Join(" | ", errors.Select(e => e.ErrorMessage)))
        {
            LogLevel = LogLevel.Warning;
            Errors = errors;
        }

        public IEnumerable<IAbpStorageOptionError> Errors { get; }

        public LogLevel LogLevel { get; set; }
    }
}