using Volo.Abp.Collections;

namespace Volo.Abp.EventBus.Distributed
{
    public class DistributedEventBusOptions
    {
        public ITypeList<IEventHandler> Handlers { get; }

        public DistributedEventBusOptions()
        {
            Handlers = new TypeList<IEventHandler>();
        }
    }
}