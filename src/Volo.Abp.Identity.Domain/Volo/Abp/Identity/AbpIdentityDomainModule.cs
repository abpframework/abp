using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    public class AbpIdentityDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            //TODO: How to configure options of AddIdentity (and return value)
            services.AddIdentity<IdentityUser, IdentityRole>();
            services.AddAssemblyOf<AbpIdentityDomainModule>();
        }
    }
}