using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
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
                options.WithDefaultRepositories();
                //options.WithCustomRepository<IdentityUser, EfCoreIdentityUserRepository>();
            });

            services.AddAssemblyOf<AbpIdentityServerEntityFrameworkCoreModule>();
        }
    }
}
