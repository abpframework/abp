using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    [DependsOn(
        typeof(AbpIdentityApplicationModule), 
        typeof(AbpIdentityDomainTestModule)
        )]
    public class AbpIdentityApplicationTestModule : AbpModule
    {

    }
}
