using Microsoft.Extensions.Logging;
using Volo.Abp.Logging;

namespace Volo.Abp.Storage
{
    public class BadStoreProviderException : AbpException, IHasLogLevel
    {
        public BadStoreProviderException(string providerName, string storeName)
            : base(
                $"The store '{storeName}' was not configured with the provider '{providerName}'. Unable to build it.")
        {
            LogLevel = LogLevel.Error;
        }

        public LogLevel LogLevel { get; set; }
    }
}