using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Prismjs;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Docs.Bundling;
using Volo.Docs.HtmlConverting;
using Volo.Docs.Localization;
using Volo.Docs.Markdown;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsApplicationContractsModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetCoreMvcUiBootstrapModule),
        typeof(AbpAspNetCoreMvcUiThemeSharedModule),
        typeof(AbpAspNetCoreMvcUiPackagesModule),
        typeof(AbpAspNetCoreMvcUiBundlingModule)
        )]
    public class DocsWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(DocsResource), typeof(DocsWebModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(DocsWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<DocsWebModule>();
            });

            var configuration = context.Services.GetConfiguration();

            Configure<RazorPagesOptions>(options =>
            {
                var docsOptions = context.Services
                    .GetRequiredServiceLazy<IOptions<DocsUiOptions>>()
                    .Value.Value;

                var routePrefix = docsOptions.RoutePrefix;

                options.Conventions.AddPageRoute("/Documents/Project/Index", routePrefix + "{projectName}");
                options.Conventions.AddPageRoute("/Documents/Project/Index", routePrefix + "{languageCode}/{projectName}");
                options.Conventions.AddPageRoute("/Documents/Project/Index", routePrefix + "{languageCode}/{projectName}/{version}/{*documentName}");
                options.Conventions.AddPageRoute("/Documents/Search", routePrefix + "search/{languageCode}/{projectName}/{version}");
            });

            context.Services.AddAutoMapperObjectMapper<DocsWebModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<DocsWebAutoMapperProfile>(validate: true);
            });

            Configure<DocumentToHtmlConverterOptions>(options =>
            {
                options.Converters[MarkdownDocumentToHtmlConverter.Type] = typeof(MarkdownDocumentToHtmlConverter);
            });

            Configure<AbpBundleContributorOptions>(options =>
            {
                options
                    .Extensions<PrismjsStyleBundleContributor>()
                    .Add<PrismjsStyleBundleContributorDocsExtension>();

                options
                    .Extensions<PrismjsScriptBundleContributor>()
                    .Add<PrismjsScriptBundleContributorDocsExtension>();
            });

            Configure<DynamicJavaScriptProxyOptions>(options =>
            {
                options.DisableModule(DocsRemoteServiceConsts.ModuleName);
            });
        }
    }
}
