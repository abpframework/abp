using System;

namespace Volo.Abp.Domain.Entities.Events;

[Serializable]
public class DomainEventEntry
{
    public object SourceEntity { get; }

    public object EventData { get; }

    public long EventOrder { get; }

    public DomainEventEntry(object sourceEntity, object eventData, long eventOrder)
    {
        SourceEntity = sourceEntity;
        EventData = eventData;
        EventOrder = eventOrder;
    }
}
