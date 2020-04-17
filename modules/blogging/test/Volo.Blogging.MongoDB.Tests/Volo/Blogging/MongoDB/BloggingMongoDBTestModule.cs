using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Volo.Blogging.MongoDB
{
    [DependsOn(
        typeof(BloggingTestBaseModule),
        typeof(BloggingMongoDbModule)
    )]
    public class BloggingMongoDbTestModule : AbpModule
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