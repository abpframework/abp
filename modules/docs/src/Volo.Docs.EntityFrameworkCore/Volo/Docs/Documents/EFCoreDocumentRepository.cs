using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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

        public async Task<List<Document>> GetListByProjectId(Guid projectId,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(d => d.ProjectId == projectId).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<List<Document>> GetAllAsync(Guid? projectId,
            string name,
            string version,
            string languageCode,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilterForGetAll(DbSet, projectId, name, version, languageCode);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? nameof(Document.Name) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetAllCountAsync(
            Guid? projectId,
            string name,
            string version,
            string languageCode,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilterForGetAll(DbSet, projectId, name, version, languageCode);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
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

        public async Task<Document> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(x => x.Id == id).SingleAsync(cancellationToken: cancellationToken);
        }

        protected virtual IQueryable<Document> ApplyFilterForGetAll(
            IQueryable<Document> query,
            Guid? projectId,
            string name,
            string version,
            string languageCode,
            CancellationToken cancellationToken = default)
        {
            return DbSet
                .WhereIf(projectId.HasValue, d => d.ProjectId == projectId.Value)
                .WhereIf(name != null, d => d.Name != null && d.Name.Contains(name))
                .WhereIf(version != null, d => d.Version != null && d.Version == version)
                .WhereIf(languageCode != null, d => d.LanguageCode != null && d.LanguageCode == languageCode);
        }
    }
}