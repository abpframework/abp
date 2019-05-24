using MyCompanyName.MyProjectName.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(MyProjectNameEntityFrameworkCoreDbMigrationsModule)
        )]
    public class MyProjectNameDbMigratorModule : AbpModule
    {
        
    }
}
