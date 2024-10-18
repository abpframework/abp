using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.Ui.LayoutHooks;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;
using Volo.CmsKit.MarkedItems;
using Volo.CmsKit.Public.Web.Menus;
using Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.GlobalResources.Script;
using Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.GlobalResources.Style;
using Volo.CmsKit.Web;
using Volo.CmsKit.Web.Icons;

namespace Volo.CmsKit.Public.Web;

[DependsOn(
    typeof(CmsKitPublicApplicationContractsModule),
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
        Configure<CmsKitUiOptions>(options =>
        {
            options.MarkedItemIcons[StandardMarkedItems.Favorite] = new LocalizableIconDictionary("fa fa-heart text-danger");
            options.MarkedItemIcons[StandardMarkedItems.Flagged] = new LocalizableIconDictionary("fa fa-flag text-info");
            options.MarkedItemIcons[StandardMarkedItems.Bookmark] = new LocalizableIconDictionary("fa fa-bookmark text-primary");
            options.MarkedItemIcons[StandardMarkedItems.Starred] = new LocalizableIconDictionary("fa fa-star text-warning");

        });
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new CmsKitPublicMenuContributor());
            options.MainMenuNames.Add(CmsKitMenus.Public);
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

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(CmsKitPublicRemoteServiceConsts.ModuleName);
        });

        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "CmsKit:";
        });

        if (GlobalFeatureManager.Instance.IsEnabled<PagesFeature>())
        {
            Configure<AbpEndpointRouterOptions>(options =>
            {
                options.EndpointConfigureActions.Add(context =>
                {
                    context.Endpoints.MapCmsPageRoute();
                });
            });
        }
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        if (GlobalFeatureManager.Instance.IsEnabled<PagesFeature>())
        {
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AddPageRoute(
                    "/Public/CmsKit/Blogs/Index",
                    CmsBlogsWebConsts.BlogsRoutePrefix.EnsureStartsWith('/') + @"/{blogSlug:minlength(1)}");

                options.Conventions.AddPageRoute(
                    "/Public/CmsKit/Blogs/BlogPost",
                    CmsBlogsWebConsts.BlogsRoutePrefix.EnsureStartsWith('/') + @"/{blogSlug}/{blogPostSlug:minlength(1)}");
            });
        }

        if (GlobalFeatureManager.Instance.IsEnabled<GlobalResourcesFeature>())
        {
            Configure<AbpLayoutHookOptions>(options =>
            {
                options.Add(
                    LayoutHooks.Head.Last,
                    typeof(GlobalStyleViewComponent)
                );
                options.Add(
                    LayoutHooks.Body.Last,
                    typeof(GlobalScriptViewComponent)
                );
            });
        }
    }
}
