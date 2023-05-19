using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.AuditLogging.MongoDB;

[DependsOn(
    typeof(AbpAuditLoggingTestBaseModule),
    typeof(AbpAuditLoggingMongoDbModule)
)]
public class AbpAuditLoggingMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
