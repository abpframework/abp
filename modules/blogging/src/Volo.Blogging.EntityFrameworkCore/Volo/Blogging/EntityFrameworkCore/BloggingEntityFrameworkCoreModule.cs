using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Blogging.EntityFrameworkCore
{
    [DependsOn(
        typeof(BloggingDomainModule),
        typeof(EntityFrameworkCoreModule))]
    public class BloggingEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BloggingDbContext>();
        }
    }
}
