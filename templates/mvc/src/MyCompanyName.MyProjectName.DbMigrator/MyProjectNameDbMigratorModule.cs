using MyCompanyName.MyProjectName.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName.DbMigrator
{
    [DependsOn(
        typeof(AutofacModule),
        typeof(MyProjectNameEntityFrameworkCoreDbMigrationsModule),
        typeof(MyProjectNameApplicationContractsModule)
        )]
    public class MyProjectNameDbMigratorModule : AbpModule
    {
        
    }
}
