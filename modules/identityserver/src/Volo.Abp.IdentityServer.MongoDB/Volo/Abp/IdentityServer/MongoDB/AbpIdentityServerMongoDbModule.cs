using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Devices;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Volo.Abp.IdentityServer.MongoDB;

[DependsOn(
    typeof(AbpIdentityServerDomainModule),
    typeof(AbpMongoDbModule)
)]
public class AbpIdentityServerMongoDbModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<IIdentityServerBuilder>(
            builder =>
            {
                builder.AddAbpStores();
            }
        );
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<AbpIdentityServerMongoDbContext>(options =>
        {
            options.AddRepository<ApiResource, MongoApiResourceRepository>();
            options.AddRepository<ApiScope, MongoApiScopeRepository>();
            options.AddRepository<IdentityResource, MongoIdentityResourceRepository>();
            options.AddRepository<Client, MongoClientRepository>();
            options.AddRepository<PersistedGrant, MongoPersistentGrantRepository>();
            options.AddRepository<DeviceFlowCodes, MongoDeviceFlowCodesRepository>();
        });
    }
}
