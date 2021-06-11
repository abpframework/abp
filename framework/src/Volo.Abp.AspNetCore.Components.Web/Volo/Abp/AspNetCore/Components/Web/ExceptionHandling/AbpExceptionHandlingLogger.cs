using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Components.ExceptionHandling;

namespace Volo.Abp.AspNetCore.Components.Web.ExceptionHandling
{
    public class AbpExceptionHandlingLogger : ILogger
    {
        private readonly IServiceCollection _serviceCollection;
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

            _userExceptionInformer.Inform(new UserExceptionInformerContext(exception));
        }

        protected virtual void TryInitialize()
        {
            var serviceProvider = _serviceCollection.GetServiceProviderOrNull();
            if (serviceProvider == null)
            {
                return;
            }

            _userExceptionInformer = serviceProvider.GetRequiredService<IUserExceptionInformer>();
        }

        public virtual bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Critical || logLevel == LogLevel.Error;
        }

        public virtual IDisposable BeginScope<TState>(TState state)
        {
            return NullDisposable.Instance;
        }
    }
}
