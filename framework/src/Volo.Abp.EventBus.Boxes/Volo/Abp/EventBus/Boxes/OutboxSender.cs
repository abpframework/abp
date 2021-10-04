using System;
using System.Threading;
using System.Threading.Tasks;
using Medallion.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Threading;

namespace Volo.Abp.EventBus.Boxes
{
    public class OutboxSender : IOutboxSender, ITransientDependency
    {
        protected IServiceProvider ServiceProvider { get; }
        protected AbpAsyncTimer Timer { get; }
        protected IDistributedEventBus DistributedEventBus { get; }
        protected IDistributedLockProvider DistributedLockProvider { get; }
        protected IEventOutbox Outbox { get; private set; }
        protected OutboxConfig OutboxConfig { get; private set; }
        protected AbpEventBusBoxesOptions EventBusBoxesOptions { get; }
        protected string DistributedLockName => "Outbox_" + OutboxConfig.Name;
        public ILogger<OutboxSender> Logger { get; set; }

        protected CancellationTokenSource StoppingTokenSource { get; }
        protected CancellationToken StoppingToken { get; }

        public OutboxSender(
            IServiceProvider serviceProvider,
            AbpAsyncTimer timer,
            IDistributedEventBus distributedEventBus,
            IDistributedLockProvider distributedLockProvider,
           IOptions<AbpEventBusBoxesOptions> eventBusBoxesOptions)
        {
            ServiceProvider = serviceProvider;
            Timer = timer;
            DistributedEventBus = distributedEventBus;
            DistributedLockProvider = distributedLockProvider;
            EventBusBoxesOptions = eventBusBoxesOptions.Value;
            Timer.Period = EventBusBoxesOptions.PeriodTimeSpan.Milliseconds;
            Timer.Elapsed += TimerOnElapsed;
            Logger = NullLogger<OutboxSender>.Instance;
            StoppingTokenSource = new CancellationTokenSource();
            StoppingToken = StoppingTokenSource.Token;
        }

        public virtual Task StartAsync(OutboxConfig outboxConfig, CancellationToken cancellationToken = default)
        {
            OutboxConfig = outboxConfig;
            Outbox = (IEventOutbox)ServiceProvider.GetRequiredService(outboxConfig.ImplementationType);
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

        private async Task TimerOnElapsed(AbpAsyncTimer arg)
        {
            await RunAsync();
        }

        protected virtual async Task RunAsync()
        {
            await using (var handle = await DistributedLockProvider.TryAcquireLockAsync(DistributedLockName, cancellationToken: StoppingToken))
            {
                if (handle != null)
                {
                    while (true)
                    {
                        var waitingEvents = await Outbox.GetWaitingEventsAsync(EventBusBoxesOptions.OutboxWaitingEventMaxCount, StoppingToken);
                        if (waitingEvents.Count <= 0)
                        {
                            break;
                        }

                        Logger.LogInformation($"Found {waitingEvents.Count} events in the outbox.");

                        foreach (var waitingEvent in waitingEvents)
                        {
                            await DistributedEventBus
                                .AsSupportsEventBoxes()
                                .PublishFromOutboxAsync(
                                    waitingEvent,
                                    OutboxConfig
                                );

                            await Outbox.DeleteAsync(waitingEvent.Id);
                            Logger.LogInformation($"Sent the event to the message broker with id = {waitingEvent.Id:N}");
                        }
                    }
                }
                else
                {
                    Logger.LogDebug("Could not obtain the distributed lock: " + DistributedLockName);
                    await TaskDelayHelper.DelayAsync(EventBusBoxesOptions.DistributedLockWaitDuration.Milliseconds, StoppingToken);
                }
            }
        }
    }
}
