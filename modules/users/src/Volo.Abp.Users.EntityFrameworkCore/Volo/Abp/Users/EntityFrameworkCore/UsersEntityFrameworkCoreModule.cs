using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.Users.EntityFrameworkCore
{
    [DependsOn(
        typeof(UsersDomainModule),
        typeof(EntityFrameworkCoreModule)
        )]
    public class UsersEntityFrameworkCoreModule : AbpModule
    {
        
    }
}
