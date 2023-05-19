using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.DistributedLocking;

[DependsOn(
    typeof(AbpDistributedLockingAbstractionsModule),
    typeof(AbpThreadingModule)
    )]
public class AbpDistributedLockingModule : AbpModule
{
}
