using BaseManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace BaseManagement
{
    [DependsOn(
        typeof(BaseManagementEntityFrameworkCoreTestModule)
        )]
    public class BaseManagementDomainTestModule : AbpModule
    {
        
    }
}
