using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Docs.Projects;

namespace Volo.Docs.MongoDB
{
    [ConnectionStringName(DocsConsts.ConnectionStringName)]
    public interface IDocsMongoDbContext : IAbpMongoDbContext
    {

        IMongoCollection<Project> Projects { get; }

    }
}