using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Modularity;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.MediaDescriptors;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitCommonApplicationContractsModule),
        typeof(CmsKitDomainModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
    )]
    public class CmsKitCommonApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<CmsKitCommonApplicationModule>(validate: true);
            });
        }
    }
}
