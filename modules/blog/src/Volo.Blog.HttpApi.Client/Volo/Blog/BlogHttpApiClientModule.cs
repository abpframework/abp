using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Blog
{
    [DependsOn(
        typeof(BlogApplicationContractsModule))]
    public class BlogHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<BlogHttpApiClientModule>();
        }
    }
}
