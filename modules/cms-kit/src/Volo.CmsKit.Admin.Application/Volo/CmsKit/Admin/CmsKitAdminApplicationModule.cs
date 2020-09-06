using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.CmsKit.Admin
{
    [DependsOn(
        typeof(CmsKitAdminApplicationContractsModule),
        typeof(AbpAutoMapperModule),
        typeof(CmsKitCommonApplicationModule)
        )]
    public class CmsKitAdminApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<CmsKitAdminApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<CmsKitAdminApplicationModule>(validate: true);
            });
        }
    }
}
