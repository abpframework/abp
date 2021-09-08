using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EventBus.Boxes
{
    public static class AbpDistributedEventBusExtensions
    {
        public static IRawEventPublisher AsRawEventPublisher(this IDistributedEventBus eventBus)
        {
            var rawPublisher = eventBus as IRawEventPublisher;
            if (rawPublisher == null)
            {
                throw new AbpException($"Given type ({eventBus.GetType().AssemblyQualifiedName}) should implement {nameof(IRawEventPublisher)}!");
            }
            
            return rawPublisher;
        } 
    }
}