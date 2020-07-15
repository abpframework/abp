using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.CmsKit.Admin;
using Volo.CmsKit.Public;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitDomainModule),
        typeof(CmsKitApplicationContractsModule),
        typeof(CmsKitPublicApplicationModule),
        typeof(CmsKitAdminApplicationModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class CmsKitApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<CmsKitApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<CmsKitApplicationModule>(validate: true);
            });
        }
    }
}
