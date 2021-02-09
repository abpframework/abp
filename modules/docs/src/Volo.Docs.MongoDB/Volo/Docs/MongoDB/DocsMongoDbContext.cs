using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Docs.Projects;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;
using Volo.Docs.Documents;

namespace Volo.Docs.MongoDB
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(DocsDbProperties.ConnectionStringName)]
    public class DocsMongoDbContext : AbpMongoDbContext, IDocsMongoDbContext
    {
        public IMongoCollection<Project> Projects => Collection<Project>();
        public IMongoCollection<Document> Documents => Collection<Document>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureDocs();
        }
    }
}
