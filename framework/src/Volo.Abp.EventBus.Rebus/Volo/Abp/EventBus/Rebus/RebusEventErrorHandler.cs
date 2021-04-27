using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus.Rebus
{
    public class RebusEventErrorHandler : EventErrorHandlerBase, ISingletonDependency
    {
        public RebusEventErrorHandler(
            IOptions<AbpEventBusOptions> options)
            : base(options)
        {
        }

        protected override Task Retry(EventExecutionErrorContext context)
        {
            Throw(context);

            return Task.CompletedTask;
        }

        protected override Task MoveToDeadLetter(EventExecutionErrorContext context)
        {
            Throw(context);

            return Task.CompletedTask;
        }

        private void Throw(EventExecutionErrorContext context)
        {
            // Rebus will automatic retries and error handling: https://github.com/rebus-org/Rebus/wiki/Automatic-retries-and-error-handling

            if (context.Exceptions.Count == 1)
            {
                context.Exceptions[0].ReThrow();
            }

            throw new AggregateException(
                "More than one error has occurred while triggering the event: " + context.EventType,
                context.Exceptions);
        }
    }
}
