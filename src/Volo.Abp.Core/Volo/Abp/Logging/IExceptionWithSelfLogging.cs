using Microsoft.Extensions.Logging;

namespace Volo.Abp.Logging
{
    public interface IExceptionWithSelfLogging
    {
        void Log(ILogger logger);
    }
}