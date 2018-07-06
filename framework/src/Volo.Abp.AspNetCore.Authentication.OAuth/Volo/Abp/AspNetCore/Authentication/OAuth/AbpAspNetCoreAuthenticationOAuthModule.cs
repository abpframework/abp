using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace Volo.Abp.AspNetCore.Authentication.OAuth
{
    [DependsOn(typeof(AbpSecurityModule))]
    public class AbpAspNetCoreAuthenticationOAuthModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpAspNetCoreAuthenticationOAuthModule>();
        }
    }
}
