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

        protected bool IsDisposed { get; private set; }
        
        protected TimeSpan ChannelDisposeWaitDuration { get; set; } = TimeSpan.FromSeconds(15);

        public ChannelPool(IConnectionPool connectionPool)
        {
            ConnectionPool = connectionPool;
            Channels = new ConcurrentDictionary<string, ChannelPoolItem>();
        }

        public virtual IChannelAccessor Acquire(string channelName = null)
        {
            CheckDisposed();

            channelName = channelName ?? "";

            var poolItem = Channels.GetOrAdd(
                channelName,
                _ => new ChannelPoolItem(CreateChannel(channelName))
            );

            poolItem.Acquire();

            return new ChannelAccessor(
                poolItem.Channel,
                () => poolItem.Release()
            );
        }

        protected virtual IModel CreateChannel(string channelName)
        {
            //TODO: How to determine the right connection name?
            return ConnectionPool.Get().CreateModel();
        }

        protected void CheckDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(ChannelPool));
            }
        }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            foreach (var poolItem in Channels.Values)
            {
                try
                {
                    poolItem.WaitIfInUse(ChannelDisposeWaitDuration);
                    poolItem.Channel.Dispose();
                }
                catch
                { }
            }

            Channels.Clear();
        }

        protected class ChannelPoolItem : IDisposable
        {
            public IModel Channel { get; }

            public bool IsInUse
            {
                get => _isInUse;
                private set => _isInUse = value;
            }
            private volatile bool _isInUse;

            public ChannelPoolItem(IModel channel)
            {
                Channel = channel;
            }

            public void Acquire()
            {
                lock (this)
                {
                    while (IsInUse)
                    {
                        Monitor.Wait(this);
                    }

                    IsInUse = true;
                }
            }

            public void WaitIfInUse(TimeSpan timeout)
            {
                lock (this)
                {
                    if (!IsInUse)
                    {
                        return;
                    }

                    Monitor.Wait(this, timeout);
                }
            }

            public void Release()
            {
                lock (this)
                {
                    IsInUse = false;
                    Monitor.PulseAll(this);
                }
            }

            public void Dispose()
            {
                Channel.Dispose();
            }
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