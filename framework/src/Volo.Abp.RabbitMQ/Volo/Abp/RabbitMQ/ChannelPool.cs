using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RabbitMQ.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.RabbitMQ;

public class ChannelPool : IChannelPool, ISingletonDependency
{
    protected IConnectionPool ConnectionPool { get; }

    protected ConcurrentDictionary<string, ChannelPoolItem> Channels { get; }

    protected SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

    protected bool IsDisposed { get; private set; }

    protected TimeSpan TotalDisposeWaitDuration { get; set; } = TimeSpan.FromSeconds(10);

    public ILogger<ChannelPool> Logger { get; set; }

    public ChannelPool(IConnectionPool connectionPool)
    {
        ConnectionPool = connectionPool;
        Channels = new ConcurrentDictionary<string, ChannelPoolItem>();
        Logger = NullLogger<ChannelPool>.Instance;
    }

    public virtual async Task<IChannelAccessor> AcquireAsync(string? channelName = null, string? connectionName = null)
    {
        CheckDisposed();

        channelName = channelName ?? "";

        ChannelPoolItem poolItem;

        if (Channels.TryGetValue(channelName, out var existingChannelPoolItem))
        {
            poolItem = existingChannelPoolItem;
        }
        else
        {
            using (await Semaphore.LockAsync())
            {
                if (Channels.TryGetValue(channelName, out var existingChannelPoolItem2))
                {
                    poolItem = existingChannelPoolItem2;
                }
                else
                {
                    poolItem = new ChannelPoolItem(await CreateChannelAsync(channelName, connectionName));
                    Channels.TryAdd(channelName, poolItem);
                }
            }
        }

        poolItem.Acquire();

        if (poolItem.Channel.IsClosed)
        {
            await poolItem.DisposeAsync();
            Channels.TryRemove(channelName, out _);

            using (await Semaphore.LockAsync())
            {
                if (Channels.TryGetValue(channelName, out var existingChannelPoolItem3))
                {
                    poolItem = existingChannelPoolItem3;
                }
                else
                {
                    poolItem = new ChannelPoolItem(await CreateChannelAsync(channelName, connectionName));
                    Channels.TryAdd(channelName, poolItem);
                }
            }

            poolItem.Acquire();
        }

        return new ChannelAccessor(
            poolItem.Channel,
            channelName,
            () => poolItem.Release()
        );
    }

    protected virtual async Task<IChannel> CreateChannelAsync(string channelName, string? connectionName)
    {
        return await (await ConnectionPool
            .GetAsync(connectionName))
            .CreateChannelAsync();
    }

    protected virtual void CheckDisposed()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(ChannelPool));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (IsDisposed)
        {
            return;
        }

        IsDisposed = true;

        if (!Channels.Any())
        {
            Logger.LogDebug($"Disposed channel pool with no channels in the pool.");
            return;
        }

        var poolDisposeStopwatch = Stopwatch.StartNew();

        Logger.LogInformation($"Disposing channel pool ({Channels.Count} channels).");

        var remainingWaitDuration = TotalDisposeWaitDuration;

        foreach (var poolItem in Channels.Values)
        {
            var poolItemDisposeStopwatch = Stopwatch.StartNew();

            try
            {
                poolItem.WaitIfInUse(remainingWaitDuration);
                await poolItem.DisposeAsync();
            }
            catch
            {
                // ignored
            }

            poolItemDisposeStopwatch.Stop();

            remainingWaitDuration = remainingWaitDuration > poolItemDisposeStopwatch.Elapsed
                ? remainingWaitDuration.Subtract(poolItemDisposeStopwatch.Elapsed)
                : TimeSpan.Zero;
        }

        poolDisposeStopwatch.Stop();

        Logger.LogInformation($"Disposed RabbitMQ Channel Pool ({Channels.Count} channels in {poolDisposeStopwatch.Elapsed.TotalMilliseconds:0.00} ms).");

        if (poolDisposeStopwatch.Elapsed.TotalSeconds > 5.0)
        {
            Logger.LogWarning($"Disposing RabbitMQ Channel Pool got time greather than expected: {poolDisposeStopwatch.Elapsed.TotalMilliseconds:0.00} ms.");
        }

        Channels.Clear();
    }

    protected class ChannelPoolItem : IAsyncDisposable
    {
        public IChannel Channel { get; }

        public bool IsInUse {
            get => _isInUse;
            private set => _isInUse = value;
        }
        private volatile bool _isInUse;

        public ChannelPoolItem(IChannel channel)
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

        public async ValueTask DisposeAsync()
        {
            await Channel.DisposeAsync();
        }
    }

    protected class ChannelAccessor : IChannelAccessor
    {
        public IChannel Channel { get; }

        public string Name { get; }

        private readonly Action _disposeAction;

        public ChannelAccessor(IChannel channel, string name, Action disposeAction)
        {
            _disposeAction = disposeAction;
            Name = name;
            Channel = channel;
        }

        public void Dispose()
        {
            _disposeAction.Invoke();
        }
    }
}
