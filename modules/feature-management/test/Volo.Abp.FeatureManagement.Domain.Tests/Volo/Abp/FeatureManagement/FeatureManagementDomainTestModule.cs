using Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Abp.FeatureManagement
{
    [DependsOn(
        typeof(FeatureManagementEntityFrameworkCoreTestModule)
        )]
    public class FeatureManagementDomainTestModule : AbpModule
    {
        
    }
}
