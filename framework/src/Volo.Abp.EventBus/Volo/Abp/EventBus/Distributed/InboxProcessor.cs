using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus.Boxes;

public class InboxProcessor : IInboxProcessor, ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }
    protected AbpAsyncTimer Timer { get; }
    protected IDistributedEventBus DistributedEventBus { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IClock Clock { get; }
    protected IEventInbox Inbox { get; private set; }
    protected InboxConfig InboxConfig { get; private set; }
    protected AbpEventBusBoxesOptions EventBusBoxesOptions { get; }

    protected DateTime? LastCleanTime { get; set; }

    protected string DistributedLockName => "AbpInbox_" + InboxConfig.Name;
    public ILogger<InboxProcessor> Logger { get; set; }
    protected CancellationTokenSource StoppingTokenSource { get; }
    protected CancellationToken StoppingToken { get; }

    public InboxProcessor(
        IServiceProvider serviceProvider,
        AbpAsyncTimer timer,
        IDistributedEventBus distributedEventBus,
        IAbpDistributedLock distributedLock,
        IUnitOfWorkManager unitOfWorkManager,
        IClock clock,
        IOptions<AbpEventBusBoxesOptions> eventBusBoxesOptions)
    {
        ServiceProvider = serviceProvider;
        Timer = timer;
        DistributedEventBus = distributedEventBus;
        DistributedLock = distributedLock;
        UnitOfWorkManager = unitOfWorkManager;
        Clock = clock;
        EventBusBoxesOptions = eventBusBoxesOptions.Value;
        Timer.Period = Convert.ToInt32(EventBusBoxesOptions.PeriodTimeSpan.TotalMilliseconds);
        Timer.Elapsed += TimerOnElapsed;
        Logger = NullLogger<InboxProcessor>.Instance;
        StoppingTokenSource = new CancellationTokenSource();
        StoppingToken = StoppingTokenSource.Token;
    }

    private async Task TimerOnElapsed(AbpAsyncTimer arg)
    {
        await RunAsync();
    }

    public Task StartAsync(InboxConfig inboxConfig, CancellationToken cancellationToken = default)
    {
        InboxConfig = inboxConfig;
        Inbox = (IEventInbox)ServiceProvider.GetRequiredService(inboxConfig.ImplementationType);
        Timer.Start(cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        StoppingTokenSource.Cancel();
        Timer.Stop(cancellationToken);
        StoppingTokenSource.Dispose();
        return Task.CompletedTask;
    }

    protected virtual async Task RunAsync()
    {
        if (StoppingToken.IsCancellationRequested)
        {
            return;
        }

        await using (var handle = await DistributedLock.TryAcquireAsync(DistributedLockName, cancellationToken: StoppingToken))
        {
            if (handle != null)
            {
                await DeleteOldEventsAsync();

                while (true)
                {
                    var waitingEvents = await Inbox.GetWaitingEventsAsync(EventBusBoxesOptions.InboxWaitingEventMaxCount, StoppingToken);
                    if (waitingEvents.Count <= 0)
                    {
                        break;
                    }

                    Logger.LogInformation($"Found {waitingEvents.Count} events in the inbox.");

                    foreach (var waitingEvent in waitingEvents)
                    {
                        using (var uow = UnitOfWorkManager.Begin(isTransactional: true, requiresNew: true))
                        {
                            await DistributedEventBus
                                .AsSupportsEventBoxes()
                                .ProcessFromInboxAsync(waitingEvent, InboxConfig);

                            await Inbox.MarkAsProcessedAsync(waitingEvent.Id);

                            await uow.CompleteAsync();
                        }

                        Logger.LogInformation($"Processed the incoming event with id = {waitingEvent.Id:N}");
                    }
                }
            }
            else
            {
                Logger.LogDebug("Could not obtain the distributed lock: " + DistributedLockName);
                try
                {
                    await Task.Delay(EventBusBoxesOptions.DistributedLockWaitDuration, StoppingToken);
                }
                catch (TaskCanceledException) { }
            }
        }
    }

    protected virtual async Task DeleteOldEventsAsync()
    {
        if (LastCleanTime != null && LastCleanTime + EventBusBoxesOptions.CleanOldEventTimeIntervalSpan > Clock.Now)
        {
            return;
        }

        await Inbox.DeleteOldEventsAsync();

        LastCleanTime = Clock.Now;
    }
}
