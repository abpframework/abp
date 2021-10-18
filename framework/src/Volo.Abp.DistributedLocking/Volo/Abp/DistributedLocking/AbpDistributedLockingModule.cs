using Volo.Abp.Modularity;

namespace Volo.Abp.DistributedLocking
{
    [DependsOn(
        typeof(AbpDistributedLockingAbstractionsModule)
        )]
    public class AbpDistributedLockingModule : AbpModule
    {
        
    }
}