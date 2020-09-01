using Volo.Abp.AspNetCore.Authentication.OAuth;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.Authentication.OpenIdConnect
{
    [DependsOn(
        typeof(AbpMultiTenancyModule),
        typeof(AbpAspNetCoreAuthenticationOAuthModule))]
    public class AbpAspNetCoreAuthenticationOpenIdConnectModule : AbpModule
    {

    }
}
