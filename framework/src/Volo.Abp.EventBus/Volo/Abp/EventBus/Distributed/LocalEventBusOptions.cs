using Volo.Abp.Collections;

namespace Volo.Abp.EventBus.Distributed
{
    public class DistributedEventBusOptions
    {
        public ITypeList<IEventHandler> Handlers { get; }
        public EtoMappingDictionary EtoMappings { get; set; }

        public DistributedEventBusOptions()
        {
            Handlers = new TypeList<IEventHandler>();
            EtoMappings = new EtoMappingDictionary();
        }
    }
}