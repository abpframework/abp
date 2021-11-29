using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.Users.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpUsersDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class AbpUsersEntityFrameworkCoreModule : AbpModule
    {
        
    }
}
