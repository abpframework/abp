using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nito.AsyncEx;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Volo.Abp.RabbitMQ;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    public class JobQueue<TArgs> : IJobQueue<TArgs>
    {
        protected Type JobType { get; }
        protected string JobName { get; }
        protected string QueueName { get; }

        protected IChannelAccessor ChannelAccessor { get; private set; }
        protected EventingBasicConsumer Consumer { get; private set; }

        protected IChannelPool ChannelPool { get; }
        protected AbpRabbitMqOptions RabbitMqOptions { get; }
        protected BackgroundJobOptions BackgroundJobOptions { get; }
        protected IRabbitMqSerializer Serializer { get; }
        protected IBackgroundJobExecuter JobExecuter { get; }

        protected AsyncLock SyncObj = new AsyncLock();
        protected bool IsDiposed { get; private set; }

        public JobQueue(
            IChannelPool channelPool,
            IRabbitMqSerializer serializer,
            IOptions<AbpRabbitMqOptions> options,
            IBackgroundJobExecuter jobExecuter,
            IOptions<BackgroundJobOptions> backgroundJobOptions)
        {
            Serializer = serializer;
            JobExecuter = jobExecuter;
            BackgroundJobOptions = backgroundJobOptions.Value;
            ChannelPool = channelPool;
            RabbitMqOptions = options.Value;

            JobName = BackgroundJobNameAttribute.GetName<TArgs>();
            JobType = BackgroundJobOptions.GetJobType(JobName);
            QueueName = "BackgroundJobs." + JobName; //TODO: Make prefix optional
        }

        public virtual async Task<string> Enqueue(TArgs args)
        {
            CheckDisposed();

            using (await SyncObj.LockAsync())
            {
                await EnsureInitializedAsync();

                await PublishAsync(args);

                return null;
            }
        }

        public Task StartAsync()
        {
            if (!BackgroundJobOptions.IsJobExecutionEnabled)
            {
                return Task.CompletedTask;
            }

            return EnsureInitializedAsync();
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

        public virtual Task EnsureInitializedAsync()
        {
            if (ChannelAccessor != null)
            {
                return Task.CompletedTask;
            }

            ChannelAccessor = ChannelPool.Acquire(QueueName);

            var queueOptions = RabbitMqOptions.Queues.GetOrDefault(QueueName)
                               ?? new QueueOptions(QueueName);

            queueOptions.Declare(ChannelAccessor.Channel);

            if (BackgroundJobOptions.IsJobExecutionEnabled)
            {
                Consumer = new EventingBasicConsumer(ChannelAccessor.Channel);
                Consumer.Received += MessageReceived;

                //TODO: What BasicConsume returns?
                ChannelAccessor.Channel.BasicConsume(
                    queue: QueueName,
                    autoAck: false,
                    consumer: Consumer
                );
            }

            return Task.CompletedTask;
        }

        protected virtual Task PublishAsync(TArgs args)
        {
            ChannelAccessor.Channel.BasicPublish(
                exchange: "",
                routingKey: QueueName,
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
                JobType,
                Serializer.Deserialize(ea.Body, typeof(TArgs))
            );

            JobExecuter.Execute(context);

            //TODO: How to ACK on success or Reject on failure?
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