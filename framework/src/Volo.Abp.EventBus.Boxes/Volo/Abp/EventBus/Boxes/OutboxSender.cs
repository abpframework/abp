using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus.Boxes
{
    //TODO: use distributed lock!
    public class OutboxSender : IOutboxSender, ITransientDependency
    {
        protected IServiceProvider ServiceProvider { get; }
        protected AbpTimer Timer { get; }
        protected IDistributedEventBus DistributedEventBus { get; }
        protected IEventOutbox Outbox { get; private set; }
        public ILogger<OutboxSender> Logger { get; set; }

        public OutboxSender(
            IServiceProvider serviceProvider,
            AbpTimer timer,
            IDistributedEventBus distributedEventBus)
        {
            ServiceProvider = serviceProvider;
            Timer = timer;
            DistributedEventBus = distributedEventBus;
            Timer.Period = 2000; //TODO: Config?
            Timer.Elapsed += TimerOnElapsed;
            Logger = NullLogger<OutboxSender>.Instance;
        }

        public virtual Task StartAsync(OutboxConfig outboxConfig)
        {
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
            while (true)
            {
                var waitingEvents = await Outbox.GetWaitingEventsAsync(100);
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
        }
    }
}