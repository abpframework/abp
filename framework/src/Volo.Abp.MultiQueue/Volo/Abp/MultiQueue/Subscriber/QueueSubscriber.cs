using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.MultiQueue.Options;

namespace Volo.Abp.MultiQueue.Subscriber;

public abstract class QueueSubscriber<TQueueOptions> : IQueueSubscriber<TQueueOptions> where TQueueOptions : class, IQueueOptions
{
    protected ConcurrentDictionary<string, Type> EventMap { get; set; }
    protected IServiceProvider ServiceProvider { get; set; }
    protected ILogger Logger { get; set; }

    private readonly CancellationTokenSource _cancellationTokenSource;


    public QueueSubscriber(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        Logger = serviceProvider.GetRequiredService<ILogger<QueueSubscriber<TQueueOptions>>>();
        EventMap = new ConcurrentDictionary<string, Type>();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public virtual void Start()
    {
        Task.Factory.StartNew(() =>
        {
            try
            {
                StartQueueAsync(_cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Pipeline startup failed");
                throw;
            }

        }, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    public virtual void Stop()
    {
        _cancellationTokenSource.Cancel();
    }

    protected abstract Task StartQueueAsync(CancellationToken cancellationToken = default);

    public virtual Task SubscribeAsync(string topic, Type type)
    {
        if (!type.IsAbstract && typeof(IQueueResult).IsAssignableFrom(type))
        {
            EventMap.TryAdd(topic, type);
        }
        return Task.CompletedTask;
    }

    public virtual Task SubscribeAsync<TQueueResult>(string topic) where TQueueResult : class, IQueueResult
    {
        return SubscribeAsync(topic, typeof(TQueueResult));
    }

    public virtual Task UnSubscribeAsync(string topic)
    {
        EventMap.TryRemove(topic, out _);
        return Task.CompletedTask;
    }
}
