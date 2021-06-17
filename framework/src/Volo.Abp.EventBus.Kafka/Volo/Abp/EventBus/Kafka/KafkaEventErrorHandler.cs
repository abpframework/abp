using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus.Kafka
{
    public class KafkaEventErrorHandler : EventErrorHandlerBase, ISingletonDependency
    {
        protected ILogger<KafkaEventErrorHandler> Logger { get; set; }

        public KafkaEventErrorHandler(
            IOptions<AbpEventBusOptions> options) : base(options)
        {
            Logger = NullLogger<KafkaEventErrorHandler>.Instance;
        }

        protected override async Task RetryAsync(EventExecutionErrorContext context)
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

        protected override async Task MoveToDeadLetterAsync(EventExecutionErrorContext context)
        {
            Logger.LogException(
                context.Exceptions.Count == 1 ? context.Exceptions.First() : new AggregateException(context.Exceptions),
                LogLevel.Error);

            await context.EventBus.As<KafkaDistributedEventBus>().PublishToDeadLetterAsync(
                context.EventType,
                context.EventData,
                context.GetProperty(HeadersKey).As<Headers>(),
                new Dictionary<string, object> {{"exceptions", context.Exceptions.Select(x => x.ToString()).ToList()}});
        }
    }
}
