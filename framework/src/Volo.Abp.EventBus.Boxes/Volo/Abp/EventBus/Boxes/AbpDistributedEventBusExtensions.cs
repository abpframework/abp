using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EventBus.Boxes
{
    public static class AbpDistributedEventBusExtensions
    {
        public static ISupportsEventBoxes AsSupportsEventBoxes(this IDistributedEventBus eventBus)
        {
            var supportsEventBoxes = eventBus as ISupportsEventBoxes;
            if (supportsEventBoxes == null)
            {
                throw new AbpException($"Given type ({eventBus.GetType().AssemblyQualifiedName}) should implement {nameof(ISupportsEventBoxes)}!");
            }
            
            return supportsEventBoxes;
        } 
    }
}