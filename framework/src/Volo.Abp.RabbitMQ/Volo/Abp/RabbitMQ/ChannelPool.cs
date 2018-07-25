using RabbitMQ.Client;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.RabbitMQ
{
    public class ChannelPool : IChannelPool, ISingletonDependency
    {
        protected IConnectionPool ConnectionPool { get; }

        public ChannelPool(IConnectionPool connectionPool)
        {
            ConnectionPool = connectionPool;
        }

        public virtual IChannelAccessor Acquire(string channelName = null)
        {
            //TODO: Pool channels!
            return new ChannelAccessor(
                CreateChannel(channelName)
            );
        }

        protected virtual IModel CreateChannel(string channelName)
        {
            //TODO: How to determine the right connection name?
            return ConnectionPool.Get().CreateModel();
        }

        protected class ChannelAccessor : IChannelAccessor
        {
            public IModel Channel { get; }

            public ChannelAccessor(IModel channel)
            {
                Channel = channel;
            }

            public void Dispose()
            {
                Channel.Dispose();
            }
        }
    }
}