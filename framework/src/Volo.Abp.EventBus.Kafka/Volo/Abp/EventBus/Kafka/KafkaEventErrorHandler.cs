using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus.Kafka
{
    public class KafkaEventErrorHandler : EventErrorHandlerBase, ISingletonDependency
    {
        public KafkaEventErrorHandler(
            IOptions<AbpEventBusOptions> options) : base(options)
        {
        }

        protected override async Task Retry(EventExecutionErrorContext context)
        {
            if (Options.RetryStrategyOptions.IntervalMillisecond > 0)
            {
                await Task.Delay(Options.RetryStrategyOptions.IntervalMillisecond);
            }

            context.TryGetRetryAttempt(out var retryAttempt);

            await context.EventBus.As<KafkaDistributedEventBus>().PublishAsync(
                context.EventType,
                context.EventData,
                context.GetProperty(HeadersKey).As<Headers>(),
                new Dictionary<string, object> {{RetryAttemptKey, ++retryAttempt}});
        }

        protected override async Task MoveToDeadLetter(EventExecutionErrorContext context)
        {
            await context.EventBus.As<KafkaDistributedEventBus>().PublishToDeadLetterAsync(
                context.EventType,
                context.EventData,
                context.GetProperty(HeadersKey).As<Headers>(),
                new Dictionary<string, object> {{"exceptions", context.Exceptions.Select(x => x.ToString()).ToList()}});
        }
    }
}
