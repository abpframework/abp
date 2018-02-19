using System;

namespace Volo.Abp.Domain.Entities.Events
{
    [Serializable]
    public class DomainEventEntry
    {
        public object SourceEntity { get; }

        public object EventData { get; }

        public DomainEventEntry(object sourceEntity, object eventData)
        {
            SourceEntity = sourceEntity;
            EventData = eventData;
        }
    }
}