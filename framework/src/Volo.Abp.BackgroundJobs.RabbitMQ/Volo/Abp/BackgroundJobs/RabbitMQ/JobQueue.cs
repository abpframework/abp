using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.RabbitMQ;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.RabbitMQ;

public class JobQueue<TArgs> : IJobQueue<TArgs>
{
    private const string ChannelPrefix = "JobQueue.";

    protected BackgroundJobConfiguration JobConfiguration { get; }
    protected JobQueueConfiguration QueueConfiguration { get; }
    protected IChannelAccessor? ChannelAccessor { get; private set; }
    protected AsyncEventingBasicConsumer? Consumer { get; private set; }

    public ILogger<JobQueue<TArgs>> Logger { get; set; }

    protected AbpBackgroundJobOptions AbpBackgroundJobOptions { get; }
    protected AbpRabbitMqBackgroundJobOptions AbpRabbitMqBackgroundJobOptions { get; }
    protected IChannelPool ChannelPool { get; }
    protected IRabbitMqSerializer Serializer { get; }
    protected IBackgroundJobExecuter JobExecuter { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IExceptionNotifier ExceptionNotifier { get; }

    protected SemaphoreSlim SyncObj = new SemaphoreSlim(1, 1);
    protected bool IsDiposed { get; private set; }

    public JobQueue(
        IOptions<AbpBackgroundJobOptions> backgroundJobOptions,
        IOptions<AbpRabbitMqBackgroundJobOptions> rabbitMqAbpBackgroundJobOptions,
        IChannelPool channelPool,
        IRabbitMqSerializer serializer,
        IBackgroundJobExecuter jobExecuter,
        IServiceScopeFactory serviceScopeFactory,
        IExceptionNotifier exceptionNotifier)
    {
        AbpBackgroundJobOptions = backgroundJobOptions.Value;
        AbpRabbitMqBackgroundJobOptions = rabbitMqAbpBackgroundJobOptions.Value;
        Serializer = serializer;
        JobExecuter = jobExecuter;
        ServiceScopeFactory = serviceScopeFactory;
        ExceptionNotifier = exceptionNotifier;
        ChannelPool = channelPool;

        JobConfiguration = AbpBackgroundJobOptions.GetJob(typeof(TArgs));
        QueueConfiguration = GetOrCreateJobQueueConfiguration();

        Logger = NullLogger<JobQueue<TArgs>>.Instance;
    }

    protected virtual JobQueueConfiguration GetOrCreateJobQueueConfiguration()
    {
        return AbpRabbitMqBackgroundJobOptions.JobQueues.GetOrDefault(typeof(TArgs)) ??
               new JobQueueConfiguration(
                   typeof(TArgs),
                   AbpRabbitMqBackgroundJobOptions.DefaultQueueNamePrefix + JobConfiguration.JobName,
                   AbpRabbitMqBackgroundJobOptions.DefaultDelayedQueueNamePrefix + JobConfiguration.JobName,
                   prefetchCount: AbpRabbitMqBackgroundJobOptions.PrefetchCount
               );
    }

    public virtual async Task<string?> EnqueueAsync(
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

    public virtual async Task StartAsync(CancellationToken cancellationToken = default)
    {
        CheckDisposed();

        if (!AbpBackgroundJobOptions.IsJobExecutionEnabled)
        {
            return;
        }

        using (await SyncObj.LockAsync(cancellationToken))
        {
            await EnsureInitializedAsync();
        }
    }

    public virtual Task StopAsync(CancellationToken cancellationToken = default)
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

    protected virtual async Task EnsureInitializedAsync()
    {
        if (ChannelAccessor != null && ChannelAccessor.Channel.IsOpen)
        {
            return;
        }

        ChannelAccessor = await ChannelPool.AcquireAsync(
            ChannelPrefix + QueueConfiguration.QueueName,
            QueueConfiguration.ConnectionName
        );

        var result = await QueueConfiguration.DeclareAsync(ChannelAccessor.Channel);
        Logger.LogDebug($"RabbitMQ Queue '{QueueConfiguration.QueueName}' has {result.MessageCount} messages and {result.ConsumerCount} consumers.");

        // Declare delayed queue
        await QueueConfiguration.DeclareDelayedAsync(ChannelAccessor.Channel);

        if (AbpBackgroundJobOptions.IsJobExecutionEnabled)
        {
            if (QueueConfiguration.PrefetchCount.HasValue)
            {
                await ChannelAccessor.Channel.BasicQosAsync(0, QueueConfiguration.PrefetchCount.Value, false);
            }

            Consumer = new AsyncEventingBasicConsumer(ChannelAccessor.Channel);
            Consumer.ReceivedAsync += MessageReceived;

            //TODO: What BasicConsume returns?
            await ChannelAccessor.Channel.BasicConsumeAsync(
                queue: QueueConfiguration.QueueName,
                autoAck: false,
                consumer: Consumer
            );
        }
    }

    protected virtual async Task PublishAsync(
        TArgs args,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        TimeSpan? delay = null)
    {
        //TODO: How to handle priority

        var routingKey = QueueConfiguration.QueueName;
        var basicProperties = new BasicProperties
        {
            Persistent = true
        };

        if (delay.HasValue)
        {
            routingKey = QueueConfiguration.DelayedQueueName;
            basicProperties.Expiration = delay.Value.TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
        }

        if (ChannelAccessor != null)
        {
            await ChannelAccessor.Channel.BasicPublishAsync(
                exchange: "",
                routingKey: routingKey,
                mandatory: false,
                basicProperties: basicProperties,
                body: Serializer.Serialize(args!)
            );
        }
    }

    protected virtual async Task MessageReceived(object sender, BasicDeliverEventArgs ea)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var context = new JobExecutionContext(
                scope.ServiceProvider,
                JobConfiguration.JobType,
                Serializer.Deserialize(ea.Body.ToArray(), typeof(TArgs))
            );

            try
            {
                await JobExecuter.ExecuteAsync(context);
                await ChannelAccessor!.Channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (BackgroundJobExecutionException)
            {
                //TODO: Reject like that?
                await ChannelAccessor!.Channel.BasicRejectAsync(deliveryTag: ea.DeliveryTag, requeue: true);
            }
            catch (Exception)
            {
                //TODO: Reject like that?
                await ChannelAccessor!.Channel.BasicRejectAsync(deliveryTag: ea.DeliveryTag, requeue: false);
            }
        }
    }

    protected void CheckDisposed()
    {
        if (IsDiposed)
        {
            throw new AbpException("This object is disposed!");
        }
    }
}
