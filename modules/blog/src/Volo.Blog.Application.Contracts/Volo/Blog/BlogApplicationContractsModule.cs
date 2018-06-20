using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Blog
{
    [DependsOn(typeof(BlogDomainSharedModule))]
    public class BlogApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<BlogApplicationContractsModule>();
        }
    }
}
