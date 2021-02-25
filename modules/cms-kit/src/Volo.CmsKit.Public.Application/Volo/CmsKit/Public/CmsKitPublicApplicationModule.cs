using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace Volo.CmsKit.Public
{
    [DependsOn(
        typeof(CmsKitCommonApplicationModule),
        typeof(CmsKitPublicApplicationContractsModule),
        typeof(AbpCachingModule)
        )]
    public class CmsKitPublicApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<CmsKitPublicApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<CmsKitPublicApplicationModule>(validate: true);
            });
        }
    }
}
