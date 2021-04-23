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
        public const string RetryIndexKey = "retryIndex";

        protected IKafkaSerializer Serializer { get; }
        protected KafkaDistributedEventBus EventBus { get; }
        protected IProducerPool ProducerPool { get; }
        protected AbpKafkaEventBusOptions AbpKafkaEventBusOptions { get; }

        protected string ErrorTopicName { get; }

        public KafkaEventErrorHandler(
            IOptions<AbpEventBusOptions> options,
            IKafkaSerializer serializer,
            KafkaDistributedEventBus eventBus,
            IKafkaMessageConsumerFactory consumerFactory,
            IProducerPool producerPool,
            IOptions<AbpKafkaEventBusOptions> abpKafkaEventBusOptions) : base(options)
        {
            Serializer = serializer;
            EventBus = eventBus;
            ProducerPool = producerPool;
            AbpKafkaEventBusOptions = abpKafkaEventBusOptions.Value;

            ErrorTopicName = options.Value.ErrorQueue ?? abpKafkaEventBusOptions.Value.TopicName + "_error";
            consumerFactory.Create(ErrorTopicName, string.Empty, abpKafkaEventBusOptions.Value.ConnectionName);
        }

        protected override async Task Retry(EventExecutionErrorContext context)
        {
            if (Options.RetryStrategyOptions.IntervalMillisecond > 0)
            {
                await Task.Delay(Options.RetryStrategyOptions.IntervalMillisecond);
            }

            var headers = context.GetProperty<Headers>(HeadersKey) ?? new Headers();
            var index = Serializer.Deserialize<int>(headers.GetLastBytes(RetryIndexKey));

            headers.Remove(RetryIndexKey);
            headers.Add(RetryIndexKey, Serializer.Serialize(++index));

            await EventBus.PublishAsync(context.EventType, context.EventData, headers);
        }

        protected override async Task MoveToErrorQueue(EventExecutionErrorContext context)
        {
            var producer = ProducerPool.Get(AbpKafkaEventBusOptions.ConnectionName);
            var eventName = EventNameAttribute.GetNameOrDefault(context.EventType);
            var body = Serializer.Serialize(context.EventData);

            await producer.ProduceAsync(
                AbpKafkaEventBusOptions.TopicName,
                new Message<string, byte[]>
                {
                    Key = eventName, Value = body,
                    Headers = new Headers {{"exceptions", Serializer.Serialize(context.Exceptions)}}
                });
        }

        protected override bool ShouldRetry(EventExecutionErrorContext context)
        {
            if (!base.ShouldRetry(context))
            {
                return false;
            }

            var headers = context.GetProperty<Headers>(HeadersKey);
            var index = 1;

            if (headers == null)
            {
                return true;
            }

            index = Serializer.Deserialize<int>(headers.GetLastBytes(RetryIndexKey));

            return Options.RetryStrategyOptions.Count < index;
        }
    }
}
