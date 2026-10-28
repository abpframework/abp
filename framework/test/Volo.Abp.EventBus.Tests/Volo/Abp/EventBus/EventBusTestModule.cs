using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus;

[DependsOn(typeof(AbpEventBusModule), typeof(AbpAutofacModule))]
public class EventBusTestModule : AbpModule
{
}
