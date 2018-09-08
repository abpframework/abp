using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.MySQL
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class AbpEntityFrameworkCoreMySQLModule : AbpModule
    {

    }
}
