using Microsoft.Extensions.Logging;
using Volo.Abp.Logging;

namespace Volo.Abp.Storage
{
    public class ProviderNotFoundException : AbpException, IHasLogLevel
    {
        public ProviderNotFoundException(string providerName)
            : base(
                $"The configured provider '{providerName}' was not found. Did you forget to add [DependsOn(typeof(Abp'{providerName}'StorageModule))] to yourAbpModule class?")
        {
            LogLevel = LogLevel.Error;
        }

        public LogLevel LogLevel { get; set; }
    }
}