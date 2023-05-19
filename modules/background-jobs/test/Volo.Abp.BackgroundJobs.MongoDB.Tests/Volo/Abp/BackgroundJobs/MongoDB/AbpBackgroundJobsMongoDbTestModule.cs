using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.BackgroundJobs.MongoDB;

[DependsOn(
    typeof(AbpBackgroundJobsTestBaseModule),
    typeof(AbpBackgroundJobsMongoDbModule)
    )]
public class AbpBackgroundJobsMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
