using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.DistributedLocking;

[DependsOn(
    typeof(AbpTestBaseModule),
    typeof(AbpDistributedLockingAbstractionsModule),
    typeof(AbpAutofacModule)
)]
public class AbpDistributedLockingAbstractionsTestModule : AbpModule
{

}
