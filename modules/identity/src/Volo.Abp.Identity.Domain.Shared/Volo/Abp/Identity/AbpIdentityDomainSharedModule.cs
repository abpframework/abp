using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    [DependsOn(typeof(AbpUsersDomainSharedModule))]
    [DependsOn(typeof(AbpLocalizationModule))]
    public class AbpIdentityDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<IdentityResource>("en");
            });

            services.AddAssemblyOf<AbpIdentityDomainSharedModule>();
        }
    }
}
