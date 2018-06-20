using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Blog
{
    [DependsOn(
        typeof(BlogApplicationContractsModule))]
    public class BlogHttpApiModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<BlogHttpApiModule>();
        }
    }
}
