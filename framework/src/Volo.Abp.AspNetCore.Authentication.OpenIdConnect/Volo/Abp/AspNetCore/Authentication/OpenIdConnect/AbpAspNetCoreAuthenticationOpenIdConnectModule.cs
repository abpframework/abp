using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Authentication.OAuth;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.RemoteServices;

namespace Volo.Abp.AspNetCore.Authentication.OpenIdConnect;

[DependsOn(
    typeof(AbpMultiTenancyModule),
    typeof(AbpAspNetCoreAuthenticationOAuthModule),
    typeof(AbpRemoteServicesModule)
    )]
public class AbpAspNetCoreAuthenticationOpenIdConnectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClient();
    }
}
