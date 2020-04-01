using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ExceptionHandling
{
    public class ExceptionNotifier : IExceptionNotifier, ITransientDependency
    {
        public ILogger<ExceptionNotifier> Logger { get; set; }

        protected IEnumerable<IExceptionSubscriber> ExceptionSubscribers { get; }

        public ExceptionNotifier(IEnumerable<IExceptionSubscriber> exceptionSubscribers)
        {
            ExceptionSubscribers = exceptionSubscribers;
            Logger = NullLogger<ExceptionNotifier>.Instance;
        }

        public virtual async Task NotifyAsync([NotNull] ExceptionNotificationContext context)
        {
            Check.NotNull(context, nameof(context));

            foreach (var exceptionSubscriber in ExceptionSubscribers)
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
