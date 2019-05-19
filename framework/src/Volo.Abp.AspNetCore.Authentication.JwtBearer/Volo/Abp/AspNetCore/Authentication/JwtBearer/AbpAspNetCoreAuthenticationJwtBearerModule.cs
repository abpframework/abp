using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace Volo.Abp.AspNetCore.Authentication.JwtBearer
{
    [DependsOn(typeof(AbpSecurityModule))]
    public class AbpAspNetCoreAuthenticationJwtBearerModule : AbpModule
    {

    }
}
