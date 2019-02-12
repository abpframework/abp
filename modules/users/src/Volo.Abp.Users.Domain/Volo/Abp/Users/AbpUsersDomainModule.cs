using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace Volo.Abp.Users
{
    [DependsOn(
        typeof(AbpUsersDomainSharedModule),
        typeof(AbpUsersAbstractionModule),
        typeof(AbpSecurityModule)
        )]
    public class AbpUsersDomainModule : AbpModule
    {
        
    }
}
