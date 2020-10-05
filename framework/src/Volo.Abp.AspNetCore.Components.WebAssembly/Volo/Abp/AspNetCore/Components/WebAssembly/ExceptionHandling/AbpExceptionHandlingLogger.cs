using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.ExceptionHandling
{
    public class AbpExceptionHandlingLogger : ILogger, IDisposable
    {
        private readonly IServiceCollection _serviceCollection;
        private IServiceScope _serviceScope;
        private IUserExceptionInformer _userExceptionInformer;

        public AbpExceptionHandlingLogger(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public virtual void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (exception == null)
            {
                return;
            }

            if (logLevel != LogLevel.Critical && logLevel != LogLevel.Error)
            {
                return;
            }

            TryInitialize();

            if (_userExceptionInformer == null)
            {
                return;
            }

            _userExceptionInformer.InformAsync(new UserExceptionInformerContext(exception));
        }

        protected virtual void TryInitialize()
        {
            var serviceProvider = _serviceCollection.GetServiceProviderOrNull();
            if (serviceProvider == null)
            {
                return;
            }

            _serviceScope = serviceProvider.CreateScope();
            _userExceptionInformer = _serviceScope.ServiceProvider.GetRequiredService<IUserExceptionInformer>();
        }

        public virtual bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Critical || logLevel == LogLevel.Error;
        }

        public virtual IDisposable BeginScope<TState>(TState state)
        {
            return NullDisposable.Instance;
        }

        public virtual void Dispose()
        {
            _serviceScope?.Dispose();
        }
    }
}
