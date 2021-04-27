using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Docs.Documents;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsDomainModule),
        typeof(DocsApplicationContractsModule),
        typeof(AbpCachingModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpDddApplicationModule)
        )]
    public class DocsApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<DocsApplicationModule>();
            
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<DocsApplicationAutoMapperProfile>(validate: true);
            });
            
            context.Services.TryAddSingleton<INavigationTreePostProcessor>(NullNavigationTreePostProcessor.Instance);
        }
    }
}
