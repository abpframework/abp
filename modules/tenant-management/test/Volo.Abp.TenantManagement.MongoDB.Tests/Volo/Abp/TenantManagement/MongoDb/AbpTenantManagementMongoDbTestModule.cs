using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.TenantManagement.MongoDB;

[DependsOn(
    typeof(AbpTenantManagementMongoDbModule),
    typeof(AbpTenantManagementTestBaseModule)
    )]
public class AbpTenantManagementMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
