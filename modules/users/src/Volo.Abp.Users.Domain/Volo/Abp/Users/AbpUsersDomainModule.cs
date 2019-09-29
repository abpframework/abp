using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Volo.Abp.Users
{
    [DependsOn(
        typeof(AbpUsersDomainSharedModule),
        typeof(AbpUsersAbstractionModule),
        typeof(AbpDddDomainModule)
        )]
    public class AbpUsersDomainModule : AbpModule
    {
        
    }
}
