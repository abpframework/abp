using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    [DependsOn(typeof(AbpIdentityApplicationModule), typeof(AbpAspNetCoreMvcModule))]
    public class AbpIdentityHttpApiModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpIdentityHttpApiModule>();
        }
    }
}