using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.SettingManagement.MongoDB;

[DependsOn(
    typeof(AbpSettingManagementMongoDbModule),
    typeof(AbpSettingManagementTestBaseModule)
    )]
public class AbpSettingManagementMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
