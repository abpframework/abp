using System;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement.MongoDB
{
    [DependsOn(
        typeof(AbpTenantManagementMongoDbModule),
        typeof(AbpTenantManagementTestBaseModule)
        )]
    public class AbpTenantManagementMongoDbTestModule : AbpModule
    {
        private static readonly MongoDbRunner MongoDbRunner = MongoDbRunner.Start();

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var connectionString = MongoDbRunner.ConnectionString.EnsureEndsWith('/') +
                                   "Db_" +
                                    Guid.NewGuid().ToString("N");

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connectionString;
            });
        }
    }
}
