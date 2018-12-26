using Volo.Abp.Modularity;

namespace Volo.AbpWebSite
{
    [DependsOn(
        typeof(AbpWebSiteDomainModule))]
    public class AbpWebSiteApplicationModule : AbpModule
    {
        
    }
}