using Volo.Abp.Modularity;

namespace Volo.Abp.IdentityServer
{
    [DependsOn(typeof(IdentityServerTestEntityFrameworkCoreModule))]
    public class IdentityServerDomainTestModule : AbpModule
    {

    }
}
