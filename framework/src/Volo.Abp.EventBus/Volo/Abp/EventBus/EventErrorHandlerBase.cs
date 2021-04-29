using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Volo.Abp.EventBus
{
    public abstract class EventErrorHandlerBase : IEventErrorHandler
    {
        public const string HeadersKey = "headers";
        public const string RetryAttemptKey = "retryAttempt";

        protected AbpEventBusOptions Options { get; }

        protected EventErrorHandlerBase(IOptions<AbpEventBusOptions> options)
        {
            Options = options.Value;
        }

        public virtual async Task Handle(EventExecutionErrorContext context)
        {
            if (!ShouldHandle(context))
            {
                ThrowOriginalExceptions(context);
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

            return Options.ErrorHandleSelector == null || Options.ErrorHandleSelector.Invoke(context.EventType);
        }

        protected virtual bool ShouldRetry(EventExecutionErrorContext context)
        {
            if (Options.RetryStrategyOptions == null)
            {
                return false;
            }

            if (!context.TryGetRetryAttempt(out var retryAttempt))
            {
                return false;
            }

            return Options.RetryStrategyOptions.MaxRetryAttempts > retryAttempt;
        }

        protected virtual void ThrowOriginalExceptions(EventExecutionErrorContext context)
        {
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
