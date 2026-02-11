using MyCompanyName.MyProjectName.EntityFrameworkCore;
using Volo.Abp.Autofac;
//<TEMPLATE-REMOVE IF-NOT='TIERED'>
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
//</TEMPLATE-REMOVE>
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    //<TEMPLATE-REMOVE IF-NOT='TIERED'>
    typeof(AbpCachingStackExchangeRedisModule),
    //</TEMPLATE-REMOVE>
    typeof(MyProjectNameEntityFrameworkCoreModule),
    typeof(MyProjectNameApplicationContractsModule)
    )]
public class MyProjectNameDbMigratorModule : AbpModule
{
    //<TEMPLATE-REMOVE IF-NOT='TIERED'>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "MyProjectName:"; });
    }
    //</TEMPLATE-REMOVE>
}
