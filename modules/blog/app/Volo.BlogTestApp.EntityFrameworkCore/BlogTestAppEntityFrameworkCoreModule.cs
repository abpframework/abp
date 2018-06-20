using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;
using Volo.Blog.EntityFrameworkCore.Volo.Blog.EntityFrameworkCore;

namespace Volo.BlogTestApp.EntityFrameworkCore
{
    [DependsOn(
        typeof(BlogEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule))]
    public class BlogTestAppEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<BlogTestAppEntityFrameworkCoreModule>();
        }
    }
}
