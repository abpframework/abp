using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Volo.Abp.AuditLogging.MongoDB
{
    [DependsOn(
        typeof(AbpAuditLoggingTestBaseModule),
        typeof(AbpAuditLoggingMongoDbModule)
    )]
    public class AbpAuditLoggingMongoDbTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var connectionString = MongoDbFixture.ConnectionString.EnsureEndsWith('/') +
                                   "Db_" +
                                    Guid.NewGuid().ToString("N");

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connectionString;
            });
        }
    }
}