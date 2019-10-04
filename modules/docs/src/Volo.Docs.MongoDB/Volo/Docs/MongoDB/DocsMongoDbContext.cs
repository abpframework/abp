using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Docs.Projects;
using Volo.Abp.MongoDB;

namespace Volo.Docs.MongoDB
{
    [ConnectionStringName(DocsConsts.ConnectionStringName)]
    public class DocsMongoDbContext : AbpMongoDbContext, IDocsMongoDbContext
    {
        public static string CollectionPrefix { get; set; } = DocsConsts.DefaultDbTablePrefix;

        public IMongoCollection<Project> Projects => Collection<Project>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureDocs(options =>
            {
                options.CollectionPrefix = CollectionPrefix;
            });
        }
    }
}
