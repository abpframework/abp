using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Application;
using Volo.Abp.Mapperly;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Docs.Common;
using Volo.Docs.Documents;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsDomainModule),
        typeof(DocsApplicationContractsModule),
        typeof(AbpCachingModule),
        typeof(AbpMapperlyModule),
        typeof(DocsCommonApplicationModule),
        typeof(AbpDddApplicationModule)
        )]
    public class DocsApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMapperlyObjectMapper<DocsApplicationModule>();
            
            context.Services.TryAddSingleton<INavigationTreePostProcessor>(NullNavigationTreePostProcessor.Instance);
        }
    }
}
