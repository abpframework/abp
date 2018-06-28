using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;
using Volo.Abp.UI;
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

            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<BloggingResource>()
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddBaseTypes(typeof(AbpUiModule))
                    .AddVirtualJson("/Localization/Resources/Blogging/Web");
            });

            services.Configure<RazorPagesOptions>(options =>
            {
                //TODO: Make configurable!
                options.Conventions.AddPageRoute("/Blog/Posts/Index", "blog/{blogShortName}");
                options.Conventions.AddPageRoute("/Blog/Posts/Detail", "blog/{blogShortName}/{postTitle}");
                options.Conventions.AddPageRoute("/Blog/Posts/Edit", "blog/{blogShortName}/posts/edit/{postId}");
                options.Conventions.AddPageRoute("/Blog/Posts/New", "blog/{blogShortName}/posts/new");
            });

            services.AddAssemblyOf<BloggingWebModule>();
        }
    }
}
