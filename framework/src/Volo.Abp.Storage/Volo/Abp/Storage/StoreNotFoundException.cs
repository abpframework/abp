using Microsoft.Extensions.Logging;
using Volo.Abp.Logging;

namespace Volo.Abp.Storage
{
    public class StoreNotFoundException : AbpException, IHasLogLevel
    {
        public StoreNotFoundException(string storeName)
            : base(
                $"The configured store '{storeName}' was not found. Did you configure it properly in AppStorage section of your appsettings.json?")
        {
            LogLevel = LogLevel.Error;
        }

        public LogLevel LogLevel { get; set; }
    }
}