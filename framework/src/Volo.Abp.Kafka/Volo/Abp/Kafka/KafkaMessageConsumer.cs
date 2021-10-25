using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Threading;

namespace Volo.Abp.Kafka
{
    public class KafkaMessageConsumer : IKafkaMessageConsumer, ITransientDependency, IDisposable
    {
        public ILogger<KafkaMessageConsumer> Logger { get; set; }

        protected IConsumerPool ConsumerPool { get; }

        protected IProducerPool ProducerPool { get; }

        protected IExceptionNotifier ExceptionNotifier { get; }

        protected AbpKafkaOptions Options { get; }

        protected AbpAsyncTimer Timer { get; }

        protected ConcurrentBag<Func<Message<string, byte[]>, Task>> Callbacks { get; }

        protected IConsumer<string, byte[]> Consumer { get; private set; }

        protected string ConnectionName { get; private set; }

        protected string GroupId { get; private set; }

        protected string TopicName { get; private set; }

        public KafkaMessageConsumer(
            IConsumerPool consumerPool,
            IExceptionNotifier exceptionNotifier,
            IOptions<AbpKafkaOptions> options,
            IProducerPool producerPool,
            AbpAsyncTimer timer)
        {
            ConsumerPool = consumerPool;
            ExceptionNotifier = exceptionNotifier;
            ProducerPool = producerPool;
            Timer = timer;
            Options = options.Value;
            Logger = NullLogger<KafkaMessageConsumer>.Instance;

            Callbacks = new ConcurrentBag<Func<Message<string, byte[]>, Task>>();

            Timer.Period = 5000; //5 sec.
            Timer.Elapsed = Timer_Elapsed;
            Timer.RunOnStart = true;
        }

        public virtual void Initialize(
            [NotNull] string topicName,
            [NotNull] string groupId,
            string connectionName = null)
        {
            Check.NotNull(topicName, nameof(topicName));
            Check.NotNull(groupId, nameof(groupId));
            TopicName = topicName;
            ConnectionName = connectionName ?? KafkaConnections.DefaultConnectionName;
            GroupId = groupId;
            Timer.Start();
        }

        public virtual void OnMessageReceived(Func<Message<string, byte[]>, Task> callback)
        {
            Callbacks.Add(callback);
        }

        protected virtual async Task Timer_Elapsed(AbpAsyncTimer timer)
        {
            await CreateTopicAsync();
            Consume();
            Timer.Stop();
        }

        protected virtual async Task CreateTopicAsync()
        {
            using (var adminClient = new AdminClientBuilder(Options.Connections.GetOrDefault(ConnectionName)).Build())
            {
                var topic = new TopicSpecification
                {
                    Name = TopicName,
                    NumPartitions = 1,
                    ReplicationFactor = 1
                };

                Options.ConfigureTopic?.Invoke(topic);

                try
                {
                    await adminClient.CreateTopicsAsync(new[] {topic});
                }
                catch (CreateTopicsException e)
                {
                    if(e.Results.Any(x => x.Error.Code != ErrorCode.TopicAlreadyExists))
                    {
                        throw;
                    }
                }
            }
        }

        protected virtual void Consume()
        {
            Consumer = ConsumerPool.Get(GroupId, ConnectionName);

            Task.Factory.StartNew(async () =>
            {
                Consumer.Subscribe(TopicName);

                while (true)
                {
                    try
                    {
                        var consumeResult = Consumer.Consume();

                        if (consumeResult.IsPartitionEOF)
                        {
                            continue;
                        }

                        await HandleIncomingMessage(consumeResult);
                    }
                    catch (ConsumeException ex)
                    {
                        Logger.LogException(ex, LogLevel.Warning);
                        await ExceptionNotifier.NotifyAsync(ex, logLevel: LogLevel.Warning);
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        protected virtual async Task HandleIncomingMessage(ConsumeResult<string, byte[]> consumeResult)
        {
            try
            {
                foreach (var callback in Callbacks)
                {
                    await callback(consumeResult.Message);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                await ExceptionNotifier.NotifyAsync(ex);
            }
            finally
            {
                Consumer.Commit(consumeResult);
            }
        }

        public virtual void Dispose()
        {
            if (Consumer == null)
            {
                return;
            }

            Consumer.Close();
            Consumer.Dispose();
        }
    }
}
