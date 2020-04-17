using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Docs.Documents;
using Volo.Docs.Projects;

namespace Volo.Docs.EntityFrameworkCore
{
    [DependsOn(
        typeof(DocsDomainModule),
        typeof(AbpEntityFrameworkCoreModule))]
    public class DocsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<DocsDbContext>(options =>
            {
                options.AddRepository<Project, EfCoreProjectRepository>();
                options.AddRepository<Document, EFCoreDocumentRepository>();
            });
        }
    }
}
