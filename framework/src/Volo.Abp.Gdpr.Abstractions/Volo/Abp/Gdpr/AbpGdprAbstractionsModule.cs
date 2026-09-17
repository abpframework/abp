using Volo.Abp.Modularity;
using Volo.Abp.EventBus.Abstractions;

namespace Volo.Abp.Gdpr;

[DependsOn(
    typeof(AbpEventBusAbstractionsModule)
)]
public class AbpGdprAbstractionsModule : AbpModule
{
}