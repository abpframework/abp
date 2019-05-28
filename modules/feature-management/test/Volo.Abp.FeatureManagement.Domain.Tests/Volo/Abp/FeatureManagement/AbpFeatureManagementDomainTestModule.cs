using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement
{
    [DependsOn(
        typeof(AbpFeatureManagementEntityFrameworkCoreTestModule)
        )]
    public class AbpFeatureManagementDomainTestModule : AbpModule
    {
        
    }
}
