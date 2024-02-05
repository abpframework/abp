using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.EventBus.Local;
using Volo.Abp.MultiQueue.Options;
using Volo.Abp.MultiQueue.Subscriber;

namespace Volo.Abp.MultiQueue.Kafka;

public class KafkaQueueSubscriber<TQueueOptions> : QueueSubscriber<TQueueOptions> where TQueueOptions : class, IQueueOptions
{
    private readonly IOptions<TQueueOptions> _options;
    private readonly ILocalEventBus _localEventBus;

    protected TQueueOptions Options => _options.Value;

    public KafkaQueueSubscriber(IServiceProvider serviceProvider, IOptions<TQueueOptions> options, ILocalEventBus localEventBus) : base(serviceProvider)
    {
        _options = options;
        _localEventBus = localEventBus;
    }
    protected virtual IConsumer<Ignore, byte[]> GetConsumer(TQueueOptions options)
    {
        if (options is KafkaQueueOptions kafkaQueueOptions)
        {
            if (string.IsNullOrWhiteSpace(kafkaQueueOptions.GroupId))
                throw new ArgumentNullException($"[GroupId] can`t  null or empty.");

            if (string.IsNullOrWhiteSpace(kafkaQueueOptions.Address))
                throw new ArgumentNullException($"[Address] can`t  null or empty.");

            var consumer = new ConsumerBuilder<Ignore, byte[]>(new ConsumerConfig
            {
                BootstrapServers = kafkaQueueOptions.Address,
                GroupId = kafkaQueueOptions.GroupId,
                SaslUsername = kafkaQueueOptions.UserName,
                SaslPassword = kafkaQueueOptions.Password

            }).Build();
            return consumer;
        }
        throw new ArgumentException("Options must be [KafkaQueueOptions]");
    }

    protected override async Task StartQueueAsync(CancellationToken cancellationToken = default)
    {
        var consumer = GetConsumer(Options);
        consumer.Subscribe(EventMap.Keys);
        while (!cancellationToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(cancellationToken);
            if (consumeResult == null) continue;
            if (consumeResult.IsPartitionEOF) continue;

            if (EventMap.TryGetValue(consumeResult.Topic, out var eventType))
            {
                try
                {
                    var data = Activator.CreateInstance(eventType) as IQueueResult;
                    data.Time = consumeResult.Message.Timestamp.UtcDateTime.ToLocalTime();
                    data.Source = "Kafka";

                    var setDataFunc = eventType.GetMethod("SetData", BindingFlags.Instance | BindingFlags.NonPublic);
                    setDataFunc.Invoke(data, new object[] { consumeResult.Message.Value });

                    await _localEventBus.PublishAsync(eventType, data);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Kafka SubscribeAsync Error");
                }
            }
        }
    }
}
