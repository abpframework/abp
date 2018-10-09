using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.IdentityServer
{
    public class AbpIdentityServerDomainSharedModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<AbpIdentityServerResource>("en");
            });
        }
    }
}
