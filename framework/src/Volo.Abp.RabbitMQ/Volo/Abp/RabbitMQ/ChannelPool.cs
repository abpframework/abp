using System;
using System.Collections.Concurrent;
using System.Threading;
using RabbitMQ.Client;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.RabbitMQ
{
    public class ChannelPool : IChannelPool, ISingletonDependency
    {
        protected IConnectionPool ConnectionPool { get; }

        protected ConcurrentDictionary<string, ChannelPoolItem> Channels { get; }
        
        public ChannelPool(IConnectionPool connectionPool)
        {
            ConnectionPool = connectionPool;
            Channels = new ConcurrentDictionary<string, ChannelPoolItem>();
        }

        public virtual IChannelAccessor Acquire(string channelName = null)
        {
            channelName = channelName ?? "";

            var poolItem = Channels.GetOrAdd(channelName, _ => new ChannelPoolItem
            {
                Channel = CreateChannel(channelName)
            });

            lock (poolItem)
            {
                while (poolItem.IsInUse)
                {
                    Monitor.Wait(poolItem);
                }
                
                poolItem.IsInUse = true;
            }

            return new ChannelAccessor(
                poolItem.Channel,
                () =>
                {
                    lock (poolItem)
                    {
                        poolItem.IsInUse = false;
                        Monitor.PulseAll(poolItem);
                    }
                }
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
            private readonly Action _disposeAction;

            public ChannelAccessor(IModel channel, Action disposeAction)
            {
                _disposeAction = disposeAction;
                Channel = channel;
            }

            public void Dispose()
            {
                _disposeAction.Invoke();
            }
        }
    }
}