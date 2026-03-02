using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus;

[Dependency(ReplaceServices = true)]
public class UnitOfWorkEventPublisher : IUnitOfWorkEventPublisher, ITransientDependency
{
    private readonly ILocalEventBus _localEventBus;
    private readonly IDistributedEventBus _distributedEventBus;

    public UnitOfWorkEventPublisher(
        ILocalEventBus localEventBus,
        IDistributedEventBus distributedEventBus)
    {
        _localEventBus = localEventBus;
        _distributedEventBus = distributedEventBus;
    }

    public async Task PublishLocalEventsAsync(IEnumerable<UnitOfWorkEventRecord> localEvents)
    {
        foreach (var localEvent in localEvents)
        {
            if (localEvent.EventName != null)
            {
                await _localEventBus.PublishByNameAsync(
                    localEvent.EventName,
                    localEvent.EventData,
                    onUnitOfWorkComplete: false
                );
            }
            else
            {
                await _localEventBus.PublishAsync(
                    localEvent.EventType,
                    localEvent.EventData,
                    onUnitOfWorkComplete: false
                );
            }
        }
    }

    public async Task PublishDistributedEventsAsync(IEnumerable<UnitOfWorkEventRecord> distributedEvents)
    {
        foreach (var distributedEvent in distributedEvents)
        {
            if (distributedEvent.EventName != null)
            {
                await _distributedEventBus.PublishByNameAsync(
                    distributedEvent.EventName,
                    distributedEvent.EventData,
                    onUnitOfWorkComplete: false,
                    useOutbox: distributedEvent.UseOutbox
                );
            }
            else
            {
                await _distributedEventBus.PublishAsync(
                    distributedEvent.EventType,
                    distributedEvent.EventData,
                    onUnitOfWorkComplete: false,
                    useOutbox: distributedEvent.UseOutbox
                );
            }
        }
    }
}
