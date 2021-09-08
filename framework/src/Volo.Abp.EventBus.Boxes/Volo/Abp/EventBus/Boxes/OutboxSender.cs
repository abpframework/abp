using System;
using System.Threading.Tasks;
using Medallion.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Threading;

namespace Volo.Abp.EventBus.Boxes
{
    //TODO: use distributed lock!
    public class OutboxSender : IOutboxSender, ITransientDependency
    {
        protected IServiceProvider ServiceProvider { get; }
        protected AbpTimer Timer { get; }
        protected IDistributedEventBus DistributedEventBus { get; }
        protected IDistributedLockProvider DistributedLockProvider { get; }
        protected IEventOutbox Outbox { get; private set; }
        protected OutboxConfig OutboxConfig { get; private set; }
        protected string DistributedLockName => "Outbox_" + OutboxConfig.Name;
        public ILogger<OutboxSender> Logger { get; set; }

        public OutboxSender(
            IServiceProvider serviceProvider,
            AbpTimer timer,
            IDistributedEventBus distributedEventBus, 
            IDistributedLockProvider distributedLockProvider)
        {
            ServiceProvider = serviceProvider;
            Timer = timer;
            DistributedEventBus = distributedEventBus;
            DistributedLockProvider = distributedLockProvider;
            Timer.Period = 2000; //TODO: Config?
            Timer.Elapsed += TimerOnElapsed;
            Logger = NullLogger<OutboxSender>.Instance;
        }

        public virtual Task StartAsync(OutboxConfig outboxConfig)
        {
            OutboxConfig = outboxConfig;
            Outbox = (IEventOutbox)ServiceProvider.GetRequiredService(outboxConfig.ImplementationType);
            Timer.Start();
            return Task.CompletedTask;
        }

        public virtual Task StopAsync()
        {
            Timer.Stop();
            return Task.CompletedTask;
        }
        
        private void TimerOnElapsed(object sender, EventArgs e)
        {
            AsyncHelper.RunSync(RunAsync);
        }

        protected virtual async Task RunAsync()
        {
            await using (var handle = await DistributedLockProvider.TryAcquireLockAsync(DistributedLockName))
            {
                if (handle != null)
                {
                    Logger.LogDebug("Obtained the distributed lock: " + DistributedLockName);
                    
                    while (true)
                    {
                        var waitingEvents = await Outbox.GetWaitingEventsAsync(1000); //TODO: Config?
                        if (waitingEvents.Count <= 0)
                        {
                            break;
                        }

                        Logger.LogInformation($"Found {waitingEvents.Count} events in the outbox.");
                
                        foreach (var waitingEvent in waitingEvents)
                        {
                            await DistributedEventBus
                                .AsRawEventPublisher()
                                .PublishRawAsync(waitingEvent.Id, waitingEvent.EventName, waitingEvent.EventData);

                            await Outbox.DeleteAsync(waitingEvent.Id);
                    
                            Logger.LogInformation($"Sent the event to the message broker with id = {waitingEvent.Id:N}");
                        }
                    }

                    await Task.Delay(30000);
                }
                else
                {
                    Logger.LogDebug("Could not obtain the distributed lock: " + DistributedLockName);
                }
            }
        }
    }
}