using System.Collections.Generic;

namespace Volo.Abp.Domain.Entities.Events
{
    public class EntityChangeReport
    {
        public List<EntityChangeEntry> ChangedEntities { get; }

        public List<DomainEventEntry> DomainEvents { get; }

        public List<DomainEventEntry> DistributedEvents { get; }

        public EntityChangeReport()
        {
            ChangedEntities = new List<EntityChangeEntry>();
            DomainEvents = new List<DomainEventEntry>();
            DistributedEvents = new List<DomainEventEntry>();
        }

        public bool IsEmpty()
        {
            return ChangedEntities.Count <= 0 && 
                   DomainEvents.Count <= 0 && 
                   DistributedEvents.Count <= 0;
        }

        public override string ToString()
        {
            return $"[EntityChangeReport] ChangedEntities: {ChangedEntities.Count}, DomainEvents: {DomainEvents.Count}, DistributedEvents: {DistributedEvents.Count}";
        }
    }
}