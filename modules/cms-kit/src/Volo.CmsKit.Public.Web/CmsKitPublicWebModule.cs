using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AutoMapper;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Public.Web.Menus;
using Volo.CmsKit.Web;

namespace Volo.CmsKit.Public.Web
{
    [DependsOn(
        typeof(CmsKitPublicHttpApiModule),
        typeof(CmsKitCommonWebModule)
    )]
    public class CmsKitPublicWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(CmsKitResource),
                    typeof(CmsKitPublicWebModule).Assembly,
                    typeof(CmsKitPublicApplicationContractsModule).Assembly,
                    typeof(CmsKitCommonApplicationContractsModule).Assembly
                );
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(CmsKitPublicWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new CmsKitPublicMenuContributor());
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<CmsKitPublicWebModule>("Volo.CmsKit.Public.Web");
            });

            context.Services.AddAutoMapperObjectMapper<CmsKitPublicWebModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<CmsKitPublicWebModule>(validate: true);
            });
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            Configure<RazorPagesOptions>(options =>
            {
                if (GlobalFeatureManager.Instance.IsEnabled<PagesFeature>())
                {
                    options.Conventions.AddPageRoute("/CmsKit/Pages/Index", @"{*pageUrl:minlength(1)}");
                }
            });
        }
    }
}
