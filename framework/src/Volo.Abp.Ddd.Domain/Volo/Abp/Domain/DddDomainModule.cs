using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.EventBus;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain
{
    [DependsOn(
        typeof(AuditingModule),
        typeof(DataModule),
        typeof(EventBusModule),
        typeof(GuidsModule),
        typeof(MultiTenancyModule),
        typeof(ThreadingModule),
        typeof(TimingModule),
        typeof(UnitOfWorkModule),
        typeof(ObjectMappingModule)
        )]
    public class DddDomainModule : AbpModule
    {

    }
}
