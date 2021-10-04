using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.Docs.Projects;
using Microsoft.Extensions.DependencyInjection;
using Volo.Docs.Documents;

namespace Volo.Docs.MongoDB
{
    [DependsOn(
        typeof(DocsDomainModule),
        typeof(AbpMongoDbModule)
    )]
    public class DocsMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<DocsMongoDbContext>(options =>
            {
                options.AddRepository<Project, MongoProjectRepository>();
                options.AddRepository<Document, MongoDocumentRepository>();
            });
        }
    }
}
