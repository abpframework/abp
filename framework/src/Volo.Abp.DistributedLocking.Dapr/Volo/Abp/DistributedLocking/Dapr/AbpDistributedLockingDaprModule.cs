using Volo.Abp.Dapr;
using Volo.Abp.Modularity;

namespace Volo.Abp.DistributedLocking.Dapr;

[DependsOn(
    typeof(AbpDistributedLockingAbstractionsModule),
    typeof(AbpDaprModule))]
public class AbpDistributedLockingDaprModule : AbpModule
{
}