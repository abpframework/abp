using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus.Boxes
{
    [DependsOn(
        typeof(AbpEventBusModule)
        )]
    public class AbpEventBusBoxesModule : AbpModule
    {
        
    }
}