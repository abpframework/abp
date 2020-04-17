using Volo.Abp.Modularity;

namespace Volo.Abp.IdentityServer
{
    [DependsOn(typeof(AbpIdentityServerTestEntityFrameworkCoreModule))]
    public class AbpIdentityServerDomainTestModule : AbpModule
    {

    }
}
