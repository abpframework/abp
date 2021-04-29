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

        protected override Task Retry(EventExecutionErrorContext context)
        {
            ThrowOriginalExceptions(context);

            return Task.CompletedTask;
        }

        protected override Task MoveToDeadLetter(EventExecutionErrorContext context)
        {
            ThrowOriginalExceptions(context);

            return Task.CompletedTask;
        }
    }
}
