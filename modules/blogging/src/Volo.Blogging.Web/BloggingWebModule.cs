using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Prismjs;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.Blogging.Bundling;
using Volo.Blogging.Files;
using Volo.Blogging.Localization;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingApplicationContractsModule),
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
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BloggingWebModule>();
            });

            context.Services.AddAutoMapperObjectMapper<BloggingWebModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpBloggingWebAutoMapperProfile>(validate: true);
            });

            Configure<AbpBundleContributorOptions>(options =>
            {
                options
                    .Extensions<PrismjsStyleBundleContributor>()
                    .Add<PrismjsStyleBundleContributorBloggingExtension>();

                options
                    .Extensions<PrismjsScriptBundleContributor>()
                    .Add<PrismjsScriptBundleContributorBloggingExtension>();
            });
            
            Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("blogNameConstraint", typeof(BloggingRouteConstraint));
            });
            
            Configure<BloggingUrlOptions>(options =>
            {
                var bundlingOptions = context.Services.GetRequiredService<IOptions<AbpBundlingOptions>>().Value;
                if (bundlingOptions.Mode != BundlingMode.None)
                {
                    options.IgnoredPaths.Add(bundlingOptions.BundleFolderName);
                }
                
                options.IgnoredPaths.AddRange(new[] 
                {
                    "error", "ApplicationConfigurationScript", "ServiceProxyScript", "Languages/Switch",
                    "ApplicationLocalizationScript", "members"
                });
            });

            Configure<RazorPagesOptions>(options =>
            {
                var urlOptions = context.Services
                    .GetRequiredServiceLazy<IOptions<BloggingUrlOptions>>()
                    .Value.Value;

                var routePrefix = urlOptions.RoutePrefix;

                if (urlOptions.SingleBlogMode.Enabled)
                {
                    options.Conventions.AddPageRoute("/Blogs/Posts/Index", routePrefix);
                    options.Conventions.AddPageRoute("/Blogs/Posts/Detail", routePrefix + "{postUrl}");
                    options.Conventions.AddPageRoute("/Blogs/Posts/Edit", routePrefix + "posts/{postId}/edit");
                    options.Conventions.AddPageRoute("/Blogs/Posts/New", routePrefix + "posts/new");
                }
                else
                {
                    if (!routePrefix.IsNullOrWhiteSpace())
                    {
                        options.Conventions.AddPageRoute("/Blogs/Index", routePrefix);
                    }
                    options.Conventions.AddPageRoute("/Blogs/Posts/Index", routePrefix + "{blogShortName:blogNameConstraint}");
                    options.Conventions.AddPageRoute("/Blogs/Posts/Detail", routePrefix + "{blogShortName:blogNameConstraint}/{postUrl}");
                    options.Conventions.AddPageRoute("/Blogs/Posts/Edit", routePrefix + "{blogShortName}/posts/{postId}/edit");
                    options.Conventions.AddPageRoute("/Blogs/Posts/New", routePrefix + "{blogShortName}/posts/new");
                }
                
                options.Conventions.AddPageRoute("/Blogs/Members/Index", routePrefix + "members/{userName}");
            });

            Configure<DynamicJavaScriptProxyOptions>(options =>
            {
                options.DisableModule(BloggingRemoteServiceConsts.ModuleName);
            });

            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.FormBodyBindingIgnoredTypes.Add(typeof(FileUploadInputDto));
            });
        }
    }
}
