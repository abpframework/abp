using Volo.Abp.AspNetCore.Authentication.OpenIdConnect;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace OpenIddict.Demo.Client.Mvc;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreAuthenticationOpenIdConnectModule)
)]
public class OpenIddictMvcModule : AbpModule
{

}
