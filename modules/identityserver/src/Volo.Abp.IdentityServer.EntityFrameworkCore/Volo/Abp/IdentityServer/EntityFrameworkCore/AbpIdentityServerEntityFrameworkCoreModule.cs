using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Modularity;

namespace Volo.Abp.IdentityServer.EntityFrameworkCore
{
    [DependsOn(typeof(AbpIdentityServerDomainModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class AbpIdentityServerEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<IdentityServerDbContext>(options =>
            {
                options.AddDefaultRepositories<IIdentityServerDbContext>();

                options.AddRepository<Client, ClientRepository>();
                options.AddRepository<ApiResource, ApiResourceRepository>();
                options.AddRepository<IdentityResource, IdentityResourceRepository>();
                options.AddRepository<PersistedGrant, PersistentGrantRepository>();
                options.AddRepository<ClientCorsOrigin, ClientCorsOriginRepository>();
            });
        }
    }
}
