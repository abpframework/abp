using Volo.Abp.Collections;
using Volo.Abp.EventBus.Handlers;

namespace Volo.Abp.EventBus
{
    public class EventBusOptions
    {
        public ITypeList<IEventHandler> Handlers { get; }

        public EventBusOptions()
        {
            Handlers = new TypeList<IEventHandler>();
        }
    }
}