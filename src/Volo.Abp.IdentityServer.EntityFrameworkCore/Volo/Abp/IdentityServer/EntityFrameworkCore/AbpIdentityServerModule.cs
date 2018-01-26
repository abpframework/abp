using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Modularity;

namespace Volo.Abp.IdentityServer.EntityFrameworkCore
{
    [DependsOn(typeof(AbpIdentityServerDomainModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class AbpIdentityServerEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAbpDbContext<IdentityServerDbContext>(options =>
            {
                options.AddDefaultRepositories();
                options.AddRepository<Client, ClientRepository>();
            });

            services.AddAssemblyOf<AbpIdentityServerEntityFrameworkCoreModule>();
        }
    }
}
