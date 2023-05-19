using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.BlobStoring.Database.MongoDB;

[DependsOn(
    typeof(BlobStoringDatabaseTestBaseModule),
    typeof(BlobStoringDatabaseMongoDbModule)
)]
public class BlobStoringDatabaseMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
