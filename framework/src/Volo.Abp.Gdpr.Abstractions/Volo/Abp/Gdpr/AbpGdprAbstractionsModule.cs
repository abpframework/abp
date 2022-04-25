using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace Volo.Abp.Gdpr;

[DependsOn(typeof(AbpEventBusModule))]
public class AbpGdprAbstractionsModule : AbpModule
{
}