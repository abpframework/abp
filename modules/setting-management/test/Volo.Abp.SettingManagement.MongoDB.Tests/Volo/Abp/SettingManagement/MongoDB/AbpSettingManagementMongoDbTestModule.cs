using System;
using Mongo2Go;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Volo.Abp.SettingManagement.MongoDB
{
    [DependsOn(
        typeof(AbpSettingManagementMongoDbModule),
        typeof(AbpSettingManagementTestBaseModule)
        )]
    public class AbpSettingManagementMongoDbTestModule : AbpModule
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
