using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace MyCompanyName.MyProjectName.MongoDB;

[DependsOn(
    typeof(MyProjectNameApplicationTestModule),
    typeof(MyProjectNameMongoDbModule)
)]
public class MyProjectNameMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
