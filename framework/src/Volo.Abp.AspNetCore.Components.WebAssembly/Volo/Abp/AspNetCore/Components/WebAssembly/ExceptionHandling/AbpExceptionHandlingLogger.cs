using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.ExceptionHandling
{
    public class AbpExceptionHandlingLogger : ILogger, IDisposable
    {
        private readonly IServiceCollection _serviceCollection;
        private IServiceScope _serviceScope;
        private IUiMessageService _messageService;

        public AbpExceptionHandlingLogger(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (logLevel != LogLevel.Critical && logLevel != LogLevel.Error)
            {
                return;
            }

            TryInitialize();

            if (_messageService == null)
            {
                return;
            }

            //TODO: handle exception types
            _messageService.ErrorAsync(
                exception?.Message ?? state?.ToString() ?? "Unknown error!"
            );
        }

        private void TryInitialize()
        {
            var serviceProvider = _serviceCollection.GetServiceProviderOrNull();
            if (serviceProvider == null)
            {
                return;
            }

            _serviceScope = serviceProvider.CreateScope();
            _messageService = _serviceScope.ServiceProvider.GetRequiredService<IUiMessageService>();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Critical || logLevel == LogLevel.Error;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullDisposable.Instance;
        }

        public void Dispose()
        {
            _serviceScope?.Dispose();
        }
    }
}
