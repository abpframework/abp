using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;
using Volo.Docs.Documents;
using Volo.Docs.Projects;

namespace Volo.Docs.MongoDB
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(DocsDbProperties.ConnectionStringName)]
    public interface IDocsMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<Project> Projects { get; }

        IMongoCollection<Document> Documents { get; }
    }
}
