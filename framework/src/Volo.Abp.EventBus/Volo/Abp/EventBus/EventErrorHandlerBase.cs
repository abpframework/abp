using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Volo.Abp.EventBus
{
    public abstract class EventErrorHandlerBase : IEventErrorHandler
    {
        protected AbpEventBusOptions Options { get; }

        protected EventErrorHandlerBase(IOptions<AbpEventBusOptions> options)
        {
            Options = options.Value;
        }

        public virtual async Task Handle(EventExecutionErrorContext context)
        {
            if (!ShouldHandle(context))
            {
                if (context.Exceptions.Count == 1)
                {
                    context.Exceptions[0].ReThrow();
                }

                throw new AggregateException(
                    "More than one error has occurred while triggering the event: " + context.EventType,
                    context.Exceptions);
            }

            if (ShouldRetry(context))
            {
                await Retry(context);
                return;
            }

            await MoveToDeadLetter(context);
        }

        protected abstract Task Retry(EventExecutionErrorContext context);

        protected abstract Task MoveToDeadLetter(EventExecutionErrorContext context);

        protected virtual bool ShouldHandle(EventExecutionErrorContext context)
        {
            if (!Options.EnabledErrorHandle)
            {
                return false;
            }

            if (Options.ErrorHandleSelector != null)
            {
                return Options.ErrorHandleSelector.Invoke(context.EventType);
            }

            return true;
        }

        protected virtual bool ShouldRetry(EventExecutionErrorContext context)
        {
            return Options.RetryStrategyOptions != null;
        }
    }
}
