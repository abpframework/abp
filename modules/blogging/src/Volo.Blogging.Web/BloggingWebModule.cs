using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Blogging.Localization;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingHttpApiModule),
        typeof(AbpAspNetCoreMvcUiBootstrapModule)
    )]
    public class BloggingWebModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(BloggingResource), typeof(BloggingWebModule).Assembly);
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BloggingWebModule>("Volo.Blogging");
            });


            services.Configure<RazorPagesOptions>(options =>
            {
                //TODO: Make configurable!
                options.Conventions.AddPageRoute("/Blog/Posts/Index", "blog/{blogShortName}");
                options.Conventions.AddPageRoute("/Blog/Posts/Detail", "blog/{blogShortName}/{postTitle}");
            });

            services.AddAssemblyOf<BloggingWebModule>();
        }
    }
}
