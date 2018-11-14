using Volo.Abp.Collections;

namespace Volo.Abp.EventBus.Local
{
    public class LocalEventBusOptions
    {
        public ITypeList<IEventHandler> Handlers { get; }

        public LocalEventBusOptions()
        {
            Handlers = new TypeList<IEventHandler>();
        }
    }
}