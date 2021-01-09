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
            return await (await GetDbSetAsync()).Where(d => d.ProjectId == projectId).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<List<Document>> GetAllAsync(
            Guid? projectId,
            string name,
            string version,
            string languageCode,
            string fileName,
            string format,
            DateTime? creationTimeMin,
            DateTime? creationTimeMax,
            DateTime? lastUpdatedTimeMin,
            DateTime? lastUpdatedTimeMax,
            DateTime? lastSignificantUpdateTimeMin,
            DateTime? lastSignificantUpdateTimeMax,
            DateTime? lastCachedTimeMin,
            DateTime? lastCachedTimeMax,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilterForGetAll(
                await GetDbSetAsync(),
                projectId: projectId,
                name: name,
                version: version,
                languageCode: languageCode,
                format: format,
                fileName: fileName,
                creationTimeMin: creationTimeMin,
                creationTimeMax: creationTimeMax,
                lastUpdatedTimeMin: lastUpdatedTimeMin,
                lastUpdatedTimeMax: lastUpdatedTimeMax,
                lastSignificantUpdateTimeMin: lastSignificantUpdateTimeMin,
                lastSignificantUpdateTimeMax: lastSignificantUpdateTimeMax,
                lastCachedTimeMin: lastCachedTimeMin,
                lastCachedTimeMax: lastCachedTimeMax
            );

            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? nameof(Document.Name) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetAllCountAsync(
            Guid? projectId,
            string name,
            string version,
            string languageCode,
            string fileName,
            string format,
            DateTime? creationTimeMin,
            DateTime? creationTimeMax,
            DateTime? lastUpdatedTimeMin,
            DateTime? lastUpdatedTimeMax,
            DateTime? lastSignificantUpdateTimeMin,
            DateTime? lastSignificantUpdateTimeMax,
            DateTime? lastCachedTimeMin,
            DateTime? lastCachedTimeMax,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilterForGetAll(
                await GetDbSetAsync(),
                projectId: projectId,
                name: name,
                version: version,
                languageCode: languageCode,
                format: format,
                fileName: fileName,
                creationTimeMin: creationTimeMin,
                creationTimeMax: creationTimeMax,
                lastUpdatedTimeMin: lastUpdatedTimeMin,
                lastUpdatedTimeMax: lastUpdatedTimeMax,
                lastSignificantUpdateTimeMin: lastSignificantUpdateTimeMin,
                lastSignificantUpdateTimeMax: lastSignificantUpdateTimeMax,
                lastCachedTimeMin: lastCachedTimeMin,
                lastCachedTimeMax: lastCachedTimeMax
            );

            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<Document> FindAsync(Guid projectId, string name, string languageCode, string version,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).IncludeDetails(includeDetails)
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
            return await (await GetDbSetAsync()).Where(x => x.Id == id).SingleAsync(cancellationToken: cancellationToken);
        }

        protected virtual IQueryable<Document> ApplyFilterForGetAll(
            IQueryable<Document> query,
            Guid? projectId,
            string name,
            string version,
            string languageCode,
            string fileName,
            string format,
            DateTime? creationTimeMin,
            DateTime? creationTimeMax,
            DateTime? lastUpdatedTimeMin,
            DateTime? lastUpdatedTimeMax,
            DateTime? lastSignificantUpdateTimeMin,
            DateTime? lastSignificantUpdateTimeMax,
            DateTime? lastCachedTimeMin,
            DateTime? lastCachedTimeMax,
            CancellationToken cancellationToken = default)
        {
            return query
                .WhereIf(projectId.HasValue,
                    d => d.ProjectId == projectId.Value)
                .WhereIf(name != null,
                    d => d.Name != null && d.Name.Contains(name))
                .WhereIf(version != null,
                    d => d.Version != null && d.Version == version)
                .WhereIf(languageCode != null,
                    d => d.LanguageCode != null && d.LanguageCode == languageCode)
                .WhereIf(fileName != null,
                    d => d.FileName != null && d.FileName.Contains(fileName))
                .WhereIf(format != null,
                    d => d.Format != null && d.Format == format)
                .WhereIf(creationTimeMin.HasValue,
                    d => d.CreationTime.Date >= creationTimeMin.Value.Date)
                .WhereIf(creationTimeMax.HasValue,
                    d => d.CreationTime.Date <= creationTimeMax.Value.Date)
                .WhereIf(lastUpdatedTimeMin.HasValue,
                    d => d.LastUpdatedTime.Date >= lastUpdatedTimeMin.Value.Date)
                .WhereIf(lastUpdatedTimeMax.HasValue,
                    d => d.LastUpdatedTime.Date <= lastUpdatedTimeMax.Value.Date)
                .WhereIf(lastSignificantUpdateTimeMin.HasValue,
                    d => d.LastSignificantUpdateTime != null && d.LastSignificantUpdateTime.Value.Date >= lastSignificantUpdateTimeMin.Value.Date)
                .WhereIf(lastSignificantUpdateTimeMax.HasValue,
                    d => d.LastSignificantUpdateTime != null && d.LastSignificantUpdateTime.Value.Date <= lastSignificantUpdateTimeMax.Value.Date)
                .WhereIf(lastCachedTimeMin.HasValue,
                    d => d.LastCachedTime.Date >= lastCachedTimeMin.Value.Date)
                .WhereIf(lastCachedTimeMax.HasValue,
                    d => d.LastCachedTime.Date <= lastCachedTimeMax.Value.Date);
        }
    }
}
