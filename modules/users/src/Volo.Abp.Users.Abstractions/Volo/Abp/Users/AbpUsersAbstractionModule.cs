using Volo.Abp.Auditing;
using Volo.Abp.Domain;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Users;

//TODO: Consider to (somehow) move this to the framework to the same assemblily of ICurrentUser!

[DependsOn(
    typeof(AbpMultiTenancyModule),
    typeof(AbpEventBusModule),
    typeof(AbpDddDomainSharedModule),
    typeof(AbpAuditingContractsModule)
)]
public class AbpUsersAbstractionModule : AbpModule
{

}
