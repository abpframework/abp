using Markdig;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AutoMapper;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Public.Web.Menus;
using Volo.CmsKit.Web;

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

        context.Services
            .AddSingleton(_ => new MarkdownPipelineBuilder()
                .UseAutoLinks()
                .UseBootstrap()
                .UseGridTables()
                .UsePipeTables()
                .Build());

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(CmsKitPublicRemoteServiceConsts.ModuleName);
        });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        if (GlobalFeatureManager.Instance.IsEnabled<PagesFeature>())
        {
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AddPageRoute("/Public/CmsKit/Pages/Index", PageConsts.UrlPrefix + "{slug:minlength(1)}");
                options.Conventions.AddPageRoute("/Public/CmsKit/Blogs/Index", @"/blogs/{blogSlug:minlength(1)}");
                options.Conventions.AddPageRoute("/Public/CmsKit/Blogs/BlogPost", @"/blogs/{blogSlug}/{blogPostSlug:minlength(1)}");
            });
        }
    }
}
