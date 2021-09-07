using System.Collections.Generic;
using Volo.Abp.Collections;

namespace Volo.Abp.EventBus.Distributed
{
    public class AbpDistributedEventBusOptions
    {
        public ITypeList<IEventHandler> Handlers { get; }
        
        public List<OutboxConfig> Outboxes { get; }

        public AbpDistributedEventBusOptions()
        {
            Handlers = new TypeList<IEventHandler>();
        }
    }

    public class OutboxConfig
    {
        
    }
}