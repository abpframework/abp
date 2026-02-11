using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace OpenIddict.Demo.API;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule)
)]
public class OpenIddictApiModule : AbpModule
{

}
