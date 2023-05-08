using System;
using Volo.Abp.Data;
using Volo.Abp.Identity.MongoDB;
using Volo.Abp.IdentityServer.MongoDB;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.IdentityServer;

[DependsOn(
    typeof(AbpIdentityServerTestBaseModule),
    typeof(AbpIdentityServerMongoDbModule),
    typeof(AbpIdentityMongoDbModule)
)]
public class AbpIdentityServerMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });

    }
}
