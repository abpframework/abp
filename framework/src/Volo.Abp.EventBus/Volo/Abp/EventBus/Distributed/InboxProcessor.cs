using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus.Distributed;

public class InboxProcessor : IInboxProcessor, ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }
    protected AbpAsyncTimer Timer { get; }
    protected IDistributedEventBus DistributedEventBus { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IClock Clock { get; }
    protected IEventInbox Inbox { get; private set; } = default!;
    protected InboxConfig InboxConfig { get; private set; } = default!;
    protected AbpEventBusBoxesOptions EventBusBoxesOptions { get; }
    protected DateTime? LastCleanTime { get; set; }

    protected string DistributedLockName { get; set; } = default!;
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

    public virtual Task StartAsync(InboxConfig inboxConfig, CancellationToken cancellationToken = default)
    {
        InboxConfig = inboxConfig;
        Inbox = (IEventInbox)ServiceProvider.GetRequiredService(inboxConfig.ImplementationType);
        DistributedLockName = $"AbpInbox_{InboxConfig.DatabaseName}";
        Timer.Start(cancellationToken);
        return Task.CompletedTask;
    }

    public virtual Task StopAsync(CancellationToken cancellationToken = default)
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
                    var waitingEvents = await GetWaitingEventsAsync();
                    if (waitingEvents.Count <= 0)
                    {
                        break;
                    }

                    Logger.LogInformation($"Found {waitingEvents.Count} events in the inbox.");

                    foreach (var waitingEvent in waitingEvents)
                    {
                        Logger.LogInformation($"Start processing the incoming event with id = {waitingEvent.Id:N}");

                        try
                        {
                            using (var uow = UnitOfWorkManager.Begin(isTransactional: true, requiresNew: true))
                            {
                                await DistributedEventBus
                                    .AsSupportsEventBoxes()
                                    .ProcessFromInboxAsync(waitingEvent, InboxConfig);

                                await Inbox.MarkAsProcessedAsync(waitingEvent.Id);

                                await uow.CompleteAsync(StoppingToken);
                            }

                            Logger.LogInformation($"Processed the incoming event with id = {waitingEvent.Id:N}");
                        }
                        catch (Exception e)
                        {
                            Logger.LogError(e, $"Event with id = {waitingEvent.Id:N} processing failed.");

                            if (EventBusBoxesOptions.InboxProcessorFailurePolicy == InboxProcessorFailurePolicy.Retry)
                            {
                                throw;
                            }

                            if (EventBusBoxesOptions.InboxProcessorFailurePolicy == InboxProcessorFailurePolicy.RetryLater)
                            {
                                using (var uow = UnitOfWorkManager.Begin(isTransactional: true, requiresNew: true))
                                {
                                    if (waitingEvent.NextRetryTime != null)
                                    {
                                        waitingEvent.RetryCount++;
                                    }

                                    if (waitingEvent.RetryCount >= EventBusBoxesOptions.InboxProcessorMaxRetryCount)
                                    {
                                        Logger.LogWarning($"Event with id = {waitingEvent.Id:N} has exceeded the maximum retry count. Marking it as discarded.");

                                        await Inbox.RetryLaterAsync(waitingEvent.Id, waitingEvent.RetryCount, null);
                                        await Inbox.MarkAsDiscardAsync(waitingEvent.Id);
                                        await uow.CompleteAsync(StoppingToken);
                                        continue;
                                    }

                                    waitingEvent.NextRetryTime = GetNextRetryTime(waitingEvent.RetryCount, EventBusBoxesOptions.InboxProcessorRetryBackoffFactor);

                                    Logger.LogInformation($"Event with id = {waitingEvent.Id:N} will retry later. " +
                                                          $"Current retry count: {waitingEvent.RetryCount}, " +
                                                          $"Next retry time: {waitingEvent.NextRetryTime}, " +
                                                          $"Max retry count: {EventBusBoxesOptions.InboxProcessorMaxRetryCount}.");

                                    await Inbox.RetryLaterAsync(waitingEvent.Id, waitingEvent.RetryCount, GetNextRetryTime(waitingEvent.RetryCount, EventBusBoxesOptions.InboxProcessorRetryBackoffFactor));
                                    await uow.CompleteAsync(StoppingToken);
                                }
                                continue;
                            }

                            if (EventBusBoxesOptions.InboxProcessorFailurePolicy == InboxProcessorFailurePolicy.Discard)
                            {
                                using (var uow = UnitOfWorkManager.Begin(isTransactional: true, requiresNew: true))
                                {
                                    Logger.LogInformation($"Event with id = {waitingEvent.Id:N} will be discarded.");

                                    await Inbox.MarkAsDiscardAsync(waitingEvent.Id);
                                    await uow.CompleteAsync(StoppingToken);
                                }
                                continue;
                            }
                        }
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

    protected virtual DateTime? GetNextRetryTime(int retryCount, double factor)
    {
        var delaySeconds = factor * Math.Pow(2, retryCount);
        return DateTime.Now.AddSeconds(delaySeconds);
    }

    protected virtual async Task<List<IncomingEventInfo>> GetWaitingEventsAsync()
    {
        return await Inbox.GetWaitingEventsAsync(EventBusBoxesOptions.InboxWaitingEventMaxCount, EventBusBoxesOptions.InboxProcessorFilter, StoppingToken);
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
