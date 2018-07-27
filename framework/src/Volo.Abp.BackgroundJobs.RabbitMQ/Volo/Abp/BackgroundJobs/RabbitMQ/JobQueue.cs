using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Nito.AsyncEx;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Volo.Abp.RabbitMQ;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    //TODO: Needs refactoring

    public class JobQueue<TArgs> : IJobQueue<TArgs>
    {
        private const string ChannelPrefix = "JobQueue.";

        protected BackgroundJobConfiguration JobConfiguration { get; }
        protected JobQueueConfiguration QueueConfiguration { get; }

        protected IChannelAccessor ChannelAccessor { get; private set; }
        protected EventingBasicConsumer Consumer { get; private set; }
        
        public ILogger<JobQueue<TArgs>> Logger { get; set; }

        protected IChannelPool ChannelPool { get; }
        protected AbpRabbitMqOptions RabbitMqOptions { get; }
        protected BackgroundJobOptions BackgroundJobOptions { get; }
        protected IRabbitMqSerializer Serializer { get; }
        protected IBackgroundJobExecuter JobExecuter { get; }
        protected RabbitMqBackgroundJobOptions RabbitMqBackgroundJobOptions { get; }

        protected AsyncLock SyncObj = new AsyncLock();
        protected bool IsDiposed { get; private set; }

        public JobQueue(
            IChannelPool channelPool,
            IRabbitMqSerializer serializer,
            IOptions<AbpRabbitMqOptions> rabbitMqOptions,
            IBackgroundJobExecuter jobExecuter,
            IOptions<BackgroundJobOptions> backgroundJobOptions,
            IOptions<RabbitMqBackgroundJobOptions> rabbitMqBackgroundJobOptions)
        {
            Serializer = serializer;
            JobExecuter = jobExecuter;
            BackgroundJobOptions = backgroundJobOptions.Value;
            ChannelPool = channelPool;
            RabbitMqOptions = rabbitMqOptions.Value;
            RabbitMqBackgroundJobOptions = rabbitMqBackgroundJobOptions.Value;

            JobConfiguration = BackgroundJobOptions.GetJob(typeof(TArgs));

            QueueConfiguration = RabbitMqBackgroundJobOptions.JobQueues.GetOrDefault(typeof(TArgs)) ??
                            new JobQueueConfiguration(
                                typeof(TArgs),
                                RabbitMqBackgroundJobOptions.DefaultQueueNamePrefix + JobConfiguration.JobName
                            );

            Logger = NullLogger<JobQueue<TArgs>>.Instance;
        }

        public virtual async Task<string> EnqueueAsync(
            TArgs args,
            BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            CheckDisposed();

            using (await SyncObj.LockAsync())
            {
                await EnsureInitializedAsync();

                await PublishAsync(args, priority, delay);

                return null;
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            CheckDisposed();

            if (!BackgroundJobOptions.IsJobExecutionEnabled)
            {
                return;
            }

            using (await SyncObj.LockAsync())
            {
                await EnsureInitializedAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            Dispose();
            return Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            if (IsDiposed)
            {
                return;
            }

            IsDiposed = true;

            ChannelAccessor?.Dispose();
        }

        protected virtual Task EnsureInitializedAsync()
        {
            if (ChannelAccessor != null)
            {
                return Task.CompletedTask;
            }

            ChannelAccessor = ChannelPool.Acquire(
                ChannelPrefix + QueueConfiguration.QueueName,
                QueueConfiguration.ConnectionName
            );

            var result = QueueConfiguration.Declare(ChannelAccessor.Channel);
            Logger.LogDebug($"RabbitMQ Queue '{QueueConfiguration.QueueName}' has {result.MessageCount} messages and {result.ConsumerCount} consumers.");

            if (BackgroundJobOptions.IsJobExecutionEnabled)
            {
                Consumer = new EventingBasicConsumer(ChannelAccessor.Channel);
                Consumer.Received += MessageReceived;

                //TODO: What BasicConsume returns?
                ChannelAccessor.Channel.BasicConsume(
                    queue: QueueConfiguration.QueueName,
                    autoAck: false,
                    consumer: Consumer
                );
            }

            return Task.CompletedTask;
        }

        protected virtual Task PublishAsync(
            TArgs args, 
            BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            //TODO: How to handle priority & delay?

            ChannelAccessor.Channel.BasicPublish(
                exchange: "",
                routingKey: QueueConfiguration.QueueName,
                basicProperties: CreateBasicPropertiesToPublish(),
                body: Serializer.Serialize(args)
            );

            return Task.CompletedTask;
        }

        protected virtual IBasicProperties CreateBasicPropertiesToPublish()
        {
            var properties = ChannelAccessor.Channel.CreateBasicProperties();
            properties.Persistent = true;
            return properties;
        }

        protected virtual void MessageReceived(object sender, BasicDeliverEventArgs ea)
        {
            var context = new JobExecutionContext(
                JobConfiguration.JobType,
                Serializer.Deserialize(ea.Body, typeof(TArgs))
            );

            try
            {
                JobExecuter.Execute(context);
                ChannelAccessor.Channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (BackgroundJobExecutionException)
            {
                //TODO: Reject like that?
                ChannelAccessor.Channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
            }
            catch (Exception)
            {
                //TODO: Reject like that?
                ChannelAccessor.Channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: false);
            }
        }

        private void CheckDisposed()
        {
            if (IsDiposed)
            {
                throw new AbpException("This object is disposed!");
            }
        }
    }
}