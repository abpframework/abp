using Volo.Abp.Collections;

namespace Volo.Abp.EventBus.Local
{
    public class AbpLocalEventBusOptions
    {
        public ITypeList<IEventHandler> Handlers { get; }

        public AbpLocalEventBusOptions()
        {
            Handlers = new TypeList<IEventHandler>();
        }
    }
}