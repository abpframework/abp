using Volo.Abp.Modularity;

namespace Volo.Abp.DistributedLocking;

[DependsOn(
    typeof(AbpTestBaseModule),
    typeof(AbpDistributedLockingAbstractionsModule)
)]
public class AbpDistributedLockingAbstractionsTestModule : AbpModule
{

}
