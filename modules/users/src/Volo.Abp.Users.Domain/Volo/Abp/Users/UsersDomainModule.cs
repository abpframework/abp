using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Volo.Abp.Users
{
    [DependsOn(
        typeof(UsersDomainSharedModule),
        typeof(UsersAbstractionModule),
        typeof(DddDomainModule)
        )]
    public class UsersDomainModule : AbpModule
    {
        
    }
}
