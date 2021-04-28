using System;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Kafka;

namespace Volo.Abp.EventBus.Kafka
{
    public class KafkaEventErrorHandler : EventErrorHandlerBase, ISingletonDependency
    {
        public const string HeadersKey = "headers";
        public const string RetryAttemptKey = "retryAttempt";

        protected IKafkaSerializer Serializer { get; }

        public KafkaEventErrorHandler(
            IOptions<AbpEventBusOptions> options,
            IKafkaSerializer serializer) : base(options)
        {
            Serializer = serializer;
        }

        protected override async Task Retry(EventExecutionErrorContext context)
        {
            if (Options.RetryStrategyOptions.IntervalMillisecond > 0)
            {
                await Task.Delay(Options.RetryStrategyOptions.IntervalMillisecond);
            }

            var headers = context.GetProperty(HeadersKey).As<Headers>();

            var retryAttempt = 0;
            if (headers.Any(x => x.Key == RetryAttemptKey))
            {
                retryAttempt = Serializer.Deserialize<int>(headers.GetLastBytes(RetryAttemptKey));
            }

            headers.Remove(RetryAttemptKey);
            headers.Add(RetryAttemptKey, Serializer.Serialize(++retryAttempt));

            await context.EventBus.As<KafkaDistributedEventBus>().PublishAsync(context.EventType, context.EventData, headers);
        }

        protected override async Task MoveToDeadLetter(EventExecutionErrorContext context)
        {
            var headers = context.GetProperty(HeadersKey).As<Headers>();
            headers.Add("exceptions", Serializer.Serialize(context.Exceptions.Select(x => x.ToString()).ToList()));
            await context.EventBus.As<KafkaDistributedEventBus>().PublishToDeadLetterAsync(context.EventType, context.EventData, headers);
        }

        protected override bool ShouldRetry(EventExecutionErrorContext context)
        {
            if (!base.ShouldRetry(context))
            {
                return false;
            }

            var headers = context.GetProperty(HeadersKey).As<Headers>();

            if (headers.All(x => x.Key != RetryAttemptKey))
            {
                return true;
            }

            var retryAttempt = Serializer.Deserialize<int>(headers.GetLastBytes(RetryAttemptKey));

            return Options.RetryStrategyOptions.MaxRetryAttempts > retryAttempt;
        }
    }
}
