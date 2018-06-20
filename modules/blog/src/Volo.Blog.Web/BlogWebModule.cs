using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Blog.Localization;

namespace Volo.Blog
{
    [DependsOn(
        typeof(BlogHttpApiModule),
        typeof(AbpAspNetCoreMvcUiBootstrapModule)
    )]
    public class BlogWebModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(BlogResource), typeof(BlogWebModule).Assembly);
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BlogWebModule>("Volo.Blog");
            });
            

            services.AddAssemblyOf<BlogWebModule>();
        }
    }
}
