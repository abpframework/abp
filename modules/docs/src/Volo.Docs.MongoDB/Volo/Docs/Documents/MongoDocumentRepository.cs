using System;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Docs.MongoDB;

namespace Volo.Docs.Documents
{
    public class MongoDocumentRepository : MongoDbRepository<IDocsMongoDbContext, Document, Guid>, IDocumentRepository
    {
        public MongoDocumentRepository(IMongoDbContextProvider<IDocsMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Document> FindAsync(Guid projectId, string name, string languageCode, string version)
        {
            return await GetMongoQueryable().FirstOrDefaultAsync(x => x.ProjectId == projectId &&
                                                                      x.Name == name &&
                                                                      x.LanguageCode == languageCode &&
                                                                      x.Version == version);
        }
    }
}