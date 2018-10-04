using IdentityServer4.Models;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using Client = Volo.Abp.IdentityServer.Clients.Client;
using IdentityResource = Volo.Abp.IdentityServer.IdentityResources.IdentityResource;

namespace Volo.Abp.IdentityServer.MongoDB
{
    [DependsOn(
        typeof(AbpIdentityServerDomainModule),
        typeof(AbpMongoDbModule)
    )]
    public class AbpIdentityServerMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            AbpIdentityServerBsonClassMap.Configure();

            context.Services.AddMongoDbContext<AbpIdentityServerMongoDbContext>(options =>
            {
                options.AddRepository<ApiResource, MongoApiResourceRepository>();
                options.AddRepository<IdentityResource, MongoIdentityResourceRepository>();
                options.AddRepository<Client, MongoClientRepository>();
                options.AddRepository<PersistedGrant, MongoPersistedGrantRepository>();
            });
        }
    }
}
