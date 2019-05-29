using Volo.Abp.Modularity;
using Volo.Utils.SolutionTemplating;

namespace Volo.AbpWebSite
{
    [DependsOn(
        typeof(SolutionTemplatingModule)
        )]
    public class WebSiteDomainModule : AbpModule
    {
        
    }
}