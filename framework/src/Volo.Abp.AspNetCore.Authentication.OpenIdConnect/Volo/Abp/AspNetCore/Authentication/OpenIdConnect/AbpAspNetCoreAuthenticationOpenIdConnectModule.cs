using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security;

namespace Volo.Abp.AspNetCore.Authentication.OpenIdConnect
{
    [DependsOn(
        typeof(AbpSecurityModule),
        typeof(AbpMultiTenancyModule))]
    public class AbpAspNetCoreAuthenticationOpenIdConnectModule : AbpModule
    {

    }
}
