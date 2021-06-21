using Volo.Abp.Collections;

namespace Volo.Abp.EventBus.Distributed
{
    public class AbpDistributedEventBusOptions
    {
        public ITypeList<IEventHandler> Handlers { get; }

        public AbpDistributedEventBusOptions()
        {
            Handlers = new TypeList<IEventHandler>();
        }
    }
}