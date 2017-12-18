using Microsoft.Extensions.Logging;

namespace Volo.Abp.Logging
{
    public interface IExceptionCanLogDetails
    {
        void LogDetails(ILogger logger);
    }
}