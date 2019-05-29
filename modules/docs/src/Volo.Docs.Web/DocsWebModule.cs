using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
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
        typeof(AspNetCoreMvcUiBootstrapModule)
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

            Configure<RazorPagesOptions>(options =>
            {
                //TODO: Make configurable!
                options.Conventions.AddPageRoute("/Documents/Project/Index", "documents/{projectName}");

                options.Conventions.AddPageRoute("/Documents/Project/Index", "documents/{projectName}/{version}/{*documentName}");
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
