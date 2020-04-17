using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Docs.Documents;
using Volo.Docs.Projects;

namespace Volo.Docs.MongoDB
{
    [ConnectionStringName(DocsDbProperties.ConnectionStringName)]
    public interface IDocsMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<Project> Projects { get; }

        IMongoCollection<Document> Documents { get; }
    }
}