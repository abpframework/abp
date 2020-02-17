using System;
using System.Threading;
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

        public async Task<Document> FindAsync(Guid projectId, string name, string languageCode, string version,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.IncludeDetails(includeDetails)
                .FirstOrDefaultAsync(x =>
                    x.ProjectId == projectId && x.Name == name && x.LanguageCode == languageCode &&
                    x.Version == version,
                cancellationToken);
        }

        public async Task DeleteAsync(Guid projectId, string name, string languageCode, string version, CancellationToken cancellationToken = default)
        {
            await DeleteAsync(x =>
                x.ProjectId == projectId && x.Name == name && x.LanguageCode == languageCode &&
                x.Version == version, cancellationToken: cancellationToken);
        }
    }
}