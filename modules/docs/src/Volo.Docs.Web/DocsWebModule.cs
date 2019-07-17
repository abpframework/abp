using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Prismjs;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Docs.Bundling;
using Volo.Docs.HtmlConverting;
using Volo.Docs.Localization;
using Volo.Docs.Markdown;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsHttpApiModule),
        typeof(AbpAspNetCoreMvcUiBootstrapModule)
        )]
    public class DocsWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(DocsResource), typeof(DocsWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<DocsWebModule>("Volo.Docs");
            });

            var configuration = context.Services.GetConfiguration();

            Configure<RazorPagesOptions>(options =>
            {
                var urlOptions = context.Services
                    .GetRequiredServiceLazy<IOptions<DocsUrlOptions>>()
                    .Value.Value;

                var routePrefix = urlOptions.RoutePrefix;

                options.Conventions.AddPageRoute("/Documents/Project/Index", routePrefix + "{projectName}");
                options.Conventions.AddPageRoute("/Documents/Project/Index", routePrefix + "{languageCode}/{projectName}");
                options.Conventions.AddPageRoute("/Documents/Project/Index", routePrefix + "{languageCode}/{projectName}/{version}/{*documentName}");
            });

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<DocsWebAutoMapperProfile>(validate: true);
            });

            Configure<DocumentToHtmlConverterOptions>(options =>
            {
                options.Converters[MarkdownDocumentToHtmlConverter.Type] = typeof(MarkdownDocumentToHtmlConverter);
            });

            Configure<BundleContributorOptions>(options =>
            {
                options
                    .Extensions<PrismjsStyleBundleContributor>()
                    .Add<PrismjsStyleBundleContributorDocsExtension>();

                options
                    .Extensions<PrismjsScriptBundleContributor>()
                    .Add<PrismjsScriptBundleContributorDocsExtension>();
            });
        }
    }
}
