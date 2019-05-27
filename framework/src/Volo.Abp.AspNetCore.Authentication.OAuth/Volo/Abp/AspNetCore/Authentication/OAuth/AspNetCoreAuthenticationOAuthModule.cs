using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace Volo.Abp.AspNetCore.Authentication.OAuth
{
    [DependsOn(typeof(SecurityModule))]
    public class AspNetCoreAuthenticationOAuthModule : AbpModule
    {

    }
}
