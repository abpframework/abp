using MyCompanyName.MyProjectName.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(MyProjectNameEntityFrameworkCoreModule),
    typeof(MyProjectNameApplicationContractsModule)
    )]
public class MyProjectNameDbMigratorModule : AbpModule
{

}
