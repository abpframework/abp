using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.Blogging.Localization;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingHttpApiModule),
        typeof(AbpAspNetCoreMvcUiBootstrapModule),
        typeof(AbpAspNetCoreMvcUiBundlingModule),
        typeof(AbpAutoMapperModule)
    )]
    public class BloggingWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(BloggingResource), typeof(BloggingWebModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(BloggingWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new BloggingMenuContributor());
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BloggingWebModule>("Volo.Blogging");
            });

            context.Services.AddAutoMapperObjectMapper<BloggingWebModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpBloggingWebAutoMapperProfile>(validate: true);
            });

            Configure<RazorPagesOptions>(options =>
            {
                var urlOptions = context.Services
                    .GetRequiredServiceLazy<IOptions<BloggingUrlOptions>>()
                    .Value.Value;

                var routePrefix = urlOptions.RoutePrefix;

                options.Conventions.AddPageRoute("/Blog/Posts/Index", routePrefix + "{blogShortName}");
                options.Conventions.AddPageRoute("/Blog/Posts/Detail", routePrefix + "{blogShortName}/{postUrl}");
                options.Conventions.AddPageRoute("/Blog/Posts/Edit", routePrefix + "{blogShortName}/posts/{postId}/edit");
                options.Conventions.AddPageRoute("/Blog/Posts/New", routePrefix + "{blogShortName}/posts/new");
            });
        }
    }
}
