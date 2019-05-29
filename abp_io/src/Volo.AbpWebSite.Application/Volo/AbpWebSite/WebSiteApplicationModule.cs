using Volo.Abp.Modularity;

namespace Volo.AbpWebSite
{
    [DependsOn(
        typeof(WebSiteDomainModule))]
    public class WebSiteApplicationModule : AbpModule
    {
        
    }
}