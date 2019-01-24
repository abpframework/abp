using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;

namespace Volo.Abp.Account.Web
{
    [DependsOn(
        typeof(AbpAccountWebModule),
        typeof(AbpIdentityServerDomainModule)
        )]
    public class AbpAccountWebIdentityServerModule : AbpModule
    {

    }
}
