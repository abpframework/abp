using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.RabbitMQ;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    public class RabbitMqBackgroundJobManager : IBackgroundJobManager, ITransientDependency
    {
        protected IChannelPool ChannelPool { get; }
        protected IRabbitMqSerializer Serializer { get; }

        public RabbitMqBackgroundJobManager(IChannelPool channelPool, IRabbitMqSerializer serializer)
        {
            Serializer = serializer;
            ChannelPool = channelPool;
        }

        public Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            var jobName = BackgroundJobNameAttribute.GetName<TArgs>();
            var queueName = "BackgroundJobs." + jobName; //TODO: Make prefix optional

            using (var channelAccessor = ChannelPool.Acquire(queueName))
            {
                var properties = channelAccessor.Channel.CreateBasicProperties();
                properties.Persistent = true;

                Publish(channelAccessor.Channel, queueName, args, properties);
            }

            return null; //TODO: Can RabbitMQ return a message identifier?
        }

        private void Publish<TArgs>(
            IModel channel, 
            string queueName, 
            TArgs args, 
            IBasicProperties properties)
        {
            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: properties,
                body: Serializer.Serialize(args)
            );
        }
    }
}
