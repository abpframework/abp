using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus.Rebus
{
    /// <summary>
    /// Rebus will automatic retries and error handling: https://github.com/rebus-org/Rebus/wiki/Automatic-retries-and-error-handling
    /// </summary>
    public class RebusEventErrorHandler : EventErrorHandlerBase, ISingletonDependency
    {
        public RebusEventErrorHandler(
            IOptions<AbpEventBusOptions> options)
            : base(options)
        {
        }

        protected override Task RetryAsync(EventExecutionErrorContext context)
        {
            ThrowOriginalExceptions(context);

            return Task.CompletedTask;
        }

        protected override Task MoveToDeadLetterAsync(EventExecutionErrorContext context)
        {
            ThrowOriginalExceptions(context);

            return Task.CompletedTask;
        }
    }
}
