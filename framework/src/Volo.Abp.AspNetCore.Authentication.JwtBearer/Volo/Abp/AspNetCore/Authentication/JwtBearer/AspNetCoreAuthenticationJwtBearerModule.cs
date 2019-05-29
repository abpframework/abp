using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace Volo.Abp.AspNetCore.Authentication.JwtBearer
{
    [DependsOn(typeof(SecurityModule))]
    public class AspNetCoreAuthenticationJwtBearerModule : AbpModule
    {

    }
}
