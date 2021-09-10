using System;
using System.Threading;
using System.Threading.Tasks;
using Medallion.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus.Boxes
{
    public class InboxProcessor : IInboxProcessor, ITransientDependency
    {
        protected IServiceProvider ServiceProvider { get; }
        protected AbpTimer Timer { get; }
        protected IDistributedEventBus DistributedEventBus { get; }
        protected IDistributedLockProvider DistributedLockProvider { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected IEventInbox Inbox { get; private set; }
        protected InboxConfig InboxConfig { get; private set; }
        
        protected string DistributedLockName => "Inbox_" + InboxConfig.Name;
        public ILogger<InboxProcessor> Logger { get; set; }

        public InboxProcessor(
            IServiceProvider serviceProvider,
            AbpTimer timer,
            IDistributedEventBus distributedEventBus, 
            IDistributedLockProvider distributedLockProvider,
            IUnitOfWorkManager unitOfWorkManager)
        {
            ServiceProvider = serviceProvider;
            Timer = timer;
            DistributedEventBus = distributedEventBus;
            DistributedLockProvider = distributedLockProvider;
            UnitOfWorkManager = unitOfWorkManager;
            Timer.Period = 2000; //TODO: Config?
            Timer.Elapsed += TimerOnElapsed;
            Logger = NullLogger<InboxProcessor>.Instance;
        }
        
        private void TimerOnElapsed(object sender, EventArgs e)
        {
            AsyncHelper.RunSync(RunAsync);
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
            Timer.Stop(cancellationToken);
            return Task.CompletedTask;
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
                        var waitingEvents = await Inbox.GetWaitingEventsAsync(1000); //TODO: Config?
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
                                    .AsRawEventPublisher()
                                    .ProcessRawAsync(InboxConfig, waitingEvent.EventName, waitingEvent.EventData);
                                
                                /*
                                await DistributedEventBus
                                    .AsRawEventPublisher()
                                    .PublishRawAsync(waitingEvent.Id, waitingEvent.EventName, waitingEvent.EventData);
                                */
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
                    await Task.Delay(7000); //TODO: Can we pass a cancellation token to cancel on shutdown? (Config?)
                }
            }
        }
    }
}