using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    [DependsOn(
        typeof(IdentityApplicationModule), 
        typeof(IdentityDomainTestModule)
        )]
    public class IdentityApplicationTestModule : AbpModule
    {

    }
}
