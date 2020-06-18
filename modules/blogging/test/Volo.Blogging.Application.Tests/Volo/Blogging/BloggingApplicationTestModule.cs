using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Blogging.Admin;
using Volo.Blogging.EntityFrameworkCore;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingApplicationModule),
        typeof(BloggingAdminApplicationModule),
        typeof(BloggingEntityFrameworkCoreTestModule),
        typeof(BloggingTestBaseModule))]
    public class BloggingApplicationTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAlwaysAllowAuthorization();
        }
    }
}
