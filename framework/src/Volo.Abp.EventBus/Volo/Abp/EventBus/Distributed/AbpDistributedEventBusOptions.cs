using Volo.Abp.Collections;

namespace Volo.Abp.EventBus.Distributed
{
    public class AbpDistributedEventBusOptions
    {
        public ITypeList<IEventHandler> Handlers { get; }
        
        public OutboxConfigList Outboxes { get; }

        public AbpDistributedEventBusOptions()
        {
            Handlers = new TypeList<IEventHandler>();
            Outboxes = new OutboxConfigList();
        }
    }
}