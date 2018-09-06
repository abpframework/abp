using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.Blogging.Localization;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingHttpApiModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetCoreMvcUiBootstrapModule)
    )]
    public class BloggingWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(BloggingResource), typeof(BloggingWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<NavigationOptions>(options =>
            {
                options.MenuContributors.Add(new BloggingMenuContributor());
            });

            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BloggingWebModule>("Volo.Blogging");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<BloggingResource>()
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddBaseTypes(typeof(AbpUiModule))
                    .AddVirtualJson("/Localization/Resources/Blogging/Web");
            });

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpBloggingWebAutoMapperProfile>(validate: true);
            });

            Configure<RazorPagesOptions>(options =>
            {
                //TODO: Make configurable!
                options.Conventions.AddPageRoute("/Blog/Posts/Index", "blog/{blogShortName}");
                options.Conventions.AddPageRoute("/Blog/Posts/Detail", "blog/{blogShortName}/{postUrl}");
                options.Conventions.AddPageRoute("/Blog/Posts/Edit", "blog/{blogShortName}/posts/edit/{postId}");
                options.Conventions.AddPageRoute("/Blog/Posts/New", "blog/{blogShortName}/posts/new");
                options.Conventions.AddPageRoute("/Blog/Tags/Posts", "blog/{blogShortName}/{tagName}/posts");
            });

            context.Services.AddAssemblyOf<BloggingWebModule>();
        }
    }
}
