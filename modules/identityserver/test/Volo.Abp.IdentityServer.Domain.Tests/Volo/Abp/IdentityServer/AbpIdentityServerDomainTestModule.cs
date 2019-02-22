using Volo.Abp.Modularity;

namespace Volo.Abp.IdentityServer
{
    [DependsOn(typeof(AbpIdentityServerMongoDbTestModule))]
    public class AbpIdentityServerDomainTestModule : AbpModule
    {

    }
}
