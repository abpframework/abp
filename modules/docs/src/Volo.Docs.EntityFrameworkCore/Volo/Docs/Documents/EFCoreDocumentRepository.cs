using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Docs.EntityFrameworkCore;

namespace Volo.Docs.Documents
{
    public class EFCoreDocumentRepository : EfCoreRepository<IDocsDbContext, Document>, IDocumentRepository
    {
        public EFCoreDocumentRepository(IDbContextProvider<IDocsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Document> FindAsync(Guid projectId, string name, string languageCode, string version)
        {
            return await DbSet.FirstOrDefaultAsync(x =>
                x.ProjectId == projectId && x.Name == name && x.LanguageCode == languageCode && x.Version == version);
        }
    }
}