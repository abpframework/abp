using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;

namespace Volo.Abp.EventBus
{
    public abstract class EventErrorHandlerBase : IEventErrorHandler
    {
        protected AbpEventBusOptions Options { get; }

        public EventErrorHandlerBase(IOptions<AbpEventBusOptions> options)
        {
            Options = options.Value;
        }

        public virtual async Task Handle(EventExecutionErrorContext context)
        {
            if (!ShouldHandle(context))
            {
                return;
            }

            if (ShouldRetry(context))
            {
                await Retry(context);
                return;
            }

            await MoveToErrorQueue(context);
        }

        protected abstract Task Retry(EventExecutionErrorContext context);

        protected abstract Task MoveToErrorQueue(EventExecutionErrorContext context);

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

            return false;
        }

        protected virtual bool ShouldRetry(EventExecutionErrorContext context)
        {
            return Options.RetryStrategyOptions == null && false;
        }
    }
}
