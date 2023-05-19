using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.EventBus.Abstractions;

[DependsOn(
    typeof(AbpObjectExtendingModule)
)]
public class AbpEventBusAbstractionsModule : AbpModule
{

}
