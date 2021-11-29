using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ExceptionHandling
{
    public class ExceptionNotifier : IExceptionNotifier, ITransientDependency
    {
        public ILogger<ExceptionNotifier> Logger { get; set; }

        protected IServiceScopeFactory ServiceScopeFactory { get; }

        public ExceptionNotifier(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
            Logger = NullLogger<ExceptionNotifier>.Instance;
        }

        public virtual async Task NotifyAsync([NotNull] ExceptionNotificationContext context)
        {
            Check.NotNull(context, nameof(context));

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var exceptionSubscribers = scope.ServiceProvider
                    .GetServices<IExceptionSubscriber>();

                foreach (var exceptionSubscriber in exceptionSubscribers)
                {
                    try
                    {
                        await exceptionSubscriber.HandleAsync(context);
                    }
                    catch (Exception e)
                    {
                        Logger.LogWarning($"Exception subscriber of type {exceptionSubscriber.GetType().AssemblyQualifiedName} has thrown an exception!");
                        Logger.LogException(e, LogLevel.Warning);
                    }
                }
            }
        }
    }
}
