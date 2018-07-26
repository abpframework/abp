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

        public Task<string> EnqueueAsync<TArgs>(
            TArgs args, 
            BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            
        }
    }
}
