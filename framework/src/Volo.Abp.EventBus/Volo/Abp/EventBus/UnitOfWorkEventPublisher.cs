using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus;

[Dependency(ReplaceServices = true)]
public class UnitOfWorkEventPublisher : IUnitOfWorkEventPublisher, ITransientDependency
{
    private readonly ILocalEventBus _localEventBus;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IDistributedEventBus _distributedEventBus;
    private readonly AbpDistributedEventBusOptions _abpDistributedEventBusOptions;

    public UnitOfWorkEventPublisher(
        ILocalEventBus localEventBus,
        IUnitOfWorkManager unitOfWorkManager,
        IDistributedEventBus distributedEventBus,
        IOptions<AbpDistributedEventBusOptions> abpDistributedEventBusOptions)
    {
        _localEventBus = localEventBus;
        _unitOfWorkManager = unitOfWorkManager;
        _distributedEventBus = distributedEventBus;
        _abpDistributedEventBusOptions = abpDistributedEventBusOptions.Value;
    }

    public async Task PublishLocalEventsAsync(IEnumerable<UnitOfWorkEventRecord> localEvents)
    {
        foreach (var localEvent in localEvents)
        {
            await _localEventBus.PublishAsync(
                localEvent.EventType,
                localEvent.EventData,
                onUnitOfWorkComplete: false
            );
        }
    }

    public async Task PublishDistributedEventsAsync(IEnumerable<UnitOfWorkEventRecord> distributedEvents)
    {
        foreach (var distributedEvent in distributedEvents)
        {
            if (distributedEvent.UseOutbox)
            {
                var eventType = distributedEvent.EventType;
                var eventData = distributedEvent.EventData;

                if (_abpDistributedEventBusOptions.OutboxExist(distributedEvent.EventType))
                {
                    await _distributedEventBus.PublishAsync(
                        eventType,
                        eventData,
                        onUnitOfWorkComplete: false,
                        useOutbox: true
                    );
                }
                else
                {
                    // Fall back to publish after the UOW is completed if the outbox is unavailable.
                    if (_unitOfWorkManager.Current != null)
                    {
                        _unitOfWorkManager.Current.OnCompleted(async() => {
                            await _localEventBus.PublishAsync(eventType, eventData, onUnitOfWorkComplete: false);
                        });
                    }
                    else
                    {
                        await _localEventBus.PublishAsync(eventType, eventData, onUnitOfWorkComplete: false);
                    }
                }
            }
            else
            {
                await _distributedEventBus.PublishAsync(
                    distributedEvent.EventType,
                    distributedEvent.EventData,
                    onUnitOfWorkComplete: false,
                    useOutbox: false
                );
            }
        }
    }
}
