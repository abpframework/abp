using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Blogging.EntityFrameworkCore
{
    [DependsOn(
        typeof(BloggingDomainModule),
        typeof(AbpEntityFrameworkCoreModule))]
    public class BloggingEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAbpDbContext<BloggingDbContext>();

            services.AddAssemblyOf<BloggingEntityFrameworkCoreModule>();
        }
    }
}
