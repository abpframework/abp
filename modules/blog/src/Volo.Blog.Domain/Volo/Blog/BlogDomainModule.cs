using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Volo.Blog
{
    [DependsOn(
        typeof(BlogDomainSharedModule),
        typeof(AbpDddDomainModule))]
    public class BlogDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<BlogDomainModule>();
        }
    }
}
