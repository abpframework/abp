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

        public virtual async Task HandleAsync(EventExecutionErrorContext context)
        {
            if (!await ShouldHandleAsync(context))
            {
                ThrowOriginalExceptions(context);
            }

            if (await ShouldRetryAsync(context))
            {
                await RetryAsync(context);
                return;
            }

            await MoveToDeadLetterAsync(context);
        }

        protected abstract Task RetryAsync(EventExecutionErrorContext context);

        protected abstract Task MoveToDeadLetterAsync(EventExecutionErrorContext context);

        protected virtual Task<bool> ShouldHandleAsync(EventExecutionErrorContext context)
        {
            if (!Options.EnabledErrorHandle)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(Options.ErrorHandleSelector == null || Options.ErrorHandleSelector.Invoke(context.EventType));
        }

        protected virtual Task<bool> ShouldRetryAsync(EventExecutionErrorContext context)
        {
            if (Options.RetryStrategyOptions == null)
            {
                return Task.FromResult(false);
            }

            if (!context.TryGetRetryAttempt(out var retryAttempt))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(Options.RetryStrategyOptions.MaxRetryAttempts > retryAttempt);
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
