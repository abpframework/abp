using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Blog.Localization;

namespace Volo.Blog
{
    [DependsOn(typeof(AbpLocalizationModule))]
    public class BlogDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<BlogResource>("en");
            });

            services.AddAssemblyOf<BlogDomainSharedModule>();
        }
    }
}
