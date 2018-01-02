using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.IdentityServer.EntityFrameworkCore
{
    public class AbpIdentityServerEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpIdentityServerEntityFrameworkCoreModule>();
        }
    }
}
