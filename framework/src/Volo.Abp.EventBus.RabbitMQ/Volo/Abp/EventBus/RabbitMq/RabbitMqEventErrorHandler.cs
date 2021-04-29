using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus.RabbitMq
{
    public class RabbitMqEventErrorHandler : EventErrorHandlerBase, ISingletonDependency
    {
        public RabbitMqEventErrorHandler(
            IOptions<AbpEventBusOptions> options)
            : base(options)
        {
        }

        protected override async Task Retry(EventExecutionErrorContext context)
        {
            if (Options.RetryStrategyOptions.IntervalMillisecond > 0)
            {
                await Task.Delay(Options.RetryStrategyOptions.IntervalMillisecond);
            }

            context.TryGetRetryAttempt(out var retryAttempt);

            await context.EventBus.As<RabbitMqDistributedEventBus>().PublishAsync(
                context.EventType,
                context.EventData,
                context.GetProperty(HeadersKey).As<IBasicProperties>(),
                new Dictionary<string, object>
                {
                    {RetryAttemptKey, ++retryAttempt},
                    {"exceptions", context.Exceptions.Select(x => x.ToString()).ToList()}
                });
        }

        protected override Task MoveToDeadLetter(EventExecutionErrorContext context)
        {
            ThrowOriginalExceptions(context);

            return Task.CompletedTask;
        }
    }
}
