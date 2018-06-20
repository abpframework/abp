using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Docs.Projects;

namespace Volo.Docs.EntityFrameworkCore
{
    [DependsOn(
        typeof(DocsDomainModule),
        typeof(AbpEntityFrameworkCoreModule))]
    public class DocsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAbpDbContext<DocsDbContext>(options =>
            {
                options.AddRepository<Project, EfCoreProjectRepository>();
            });

            services.AddAssemblyOf<DocsEntityFrameworkCoreModule>();
        }
    }
}
