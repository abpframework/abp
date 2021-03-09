using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.AspNetCore.Components.Web.ExceptionHandling
{
    public class AbpExceptionHandlingLoggerProvider : ILoggerProvider
    {
        private AbpExceptionHandlingLogger _logger;
        private static readonly object SyncObj = new object();
        private readonly IServiceCollection _serviceCollection;

        public AbpExceptionHandlingLoggerProvider(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (_logger == null)
            {
                lock (SyncObj)
                {
                    if (_logger == null)
                    {
                        _logger = new AbpExceptionHandlingLogger(_serviceCollection);
                    }
                }
            }

            return _logger;
        }

        public void Dispose()
        {

        }
    }
}
