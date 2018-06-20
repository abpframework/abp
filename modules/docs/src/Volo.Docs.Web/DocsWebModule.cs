using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Docs.Localization;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsHttpApiModule),
        typeof(AbpAspNetCoreMvcUiBootstrapModule)
        )]
    public class DocsWebModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(DocsResource), typeof(DocsWebModule).Assembly);
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<DocsWebModule>("Volo.Docs");
            });

            services.Configure<RazorPagesOptions>(options =>
            {
                //TODO: Make configurable!
                options.Conventions.AddPageRoute("/Documents/Project/Index", "documents/{projectName}/{version}/{*documentName}");
            });

            services.AddAssemblyOf<DocsWebModule>();
        }
    }
}
