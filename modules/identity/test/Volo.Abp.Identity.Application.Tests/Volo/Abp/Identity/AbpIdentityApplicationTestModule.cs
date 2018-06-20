using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    [DependsOn(
        typeof(AbpIdentityApplicationModule), 
        typeof(AbpIdentityDomainTestModule)
        )]
    public class AbpIdentityApplicationTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpIdentityApplicationTestModule>();
        }
    }
}
