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

        public virtual async Task<List<DocumentWithoutDetails>> GetListWithoutDetailsByProjectId(Guid projectId, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(d => d.ProjectId == projectId)
                .Select(x => new DocumentWithoutDetails
                {
                    Id = x.Id,
                    Version = x.Version,
                    LanguageCode = x.LanguageCode,
                    Format = x.Format,
                    Name = x.Name
                })
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<DocumentInfo>> GetUniqueListDocumentInfoAsync(CancellationToken cancellationToken = default)
        {
            
            return await (await GetDbSetAsync())
                .Select(x=> new DocumentInfo
                {
                    ProjectId = x.ProjectId,
                    Version = x.Version,
                    LanguageCode = x.LanguageCode,
                    Format = x.Format,
                })
                .Distinct()
                .OrderByDescending(x=>x.Version)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }


        public virtual async Task<List<Document>> GetListByProjectId(Guid projectId,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).Where(d => d.ProjectId == projectId).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Document>> GetUniqueDocumentsByProjectIdPagedAsync(Guid projectId, int skipCount, int maxResultCount,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(d => d.ProjectId == projectId)
                .OrderBy(x => x.LastCachedTime)
                .GroupBy(x => new { x.Name, x.LanguageCode, x.Version })
                .Select(group => group.First())
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetUniqueDocumentCountByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(d => d.ProjectId == projectId)
                .GroupBy(x => new {x.FileName, x.Version, x.LanguageCode})
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task UpdateProjectLastCachedTimeAsync(Guid projectId, DateTime cachedTime,
            CancellationToken cancellationToken = default)
        {
            await (await GetDbSetAsync()).Where(d => d.ProjectId == projectId).ExecuteUpdateAsync(x => x.SetProperty(d => d.LastCachedTime, cachedTime), GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Document>> GetListAsync(Guid? projectId, string version, string name, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(version != null, x => x.Version == version)
                .WhereIf(name != null, x => x.Name == name)
                .WhereIf(projectId.HasValue, x => x.ProjectId == projectId)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<DocumentWithoutContent>> GetAllAsync(
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
            var query =await ApplyFilterForGetAll(
                await GetDbSetAsync(),
                projectId: projectId,
                name: name,
                version: version,
                languageCode: languageCode,
                fileName: fileName,
                format: format,
                creationTimeMin: creationTimeMin,
                creationTimeMax: creationTimeMax,
                lastUpdatedTimeMin: lastUpdatedTimeMin,
                lastUpdatedTimeMax: lastUpdatedTimeMax,
                lastSignificantUpdateTimeMin: lastSignificantUpdateTimeMin,
                lastSignificantUpdateTimeMax: lastSignificantUpdateTimeMax,
                lastCachedTimeMin: lastCachedTimeMin, lastCachedTimeMax: lastCachedTimeMax);

            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? nameof(Document.Name) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<long> GetAllCountAsync(
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
            var query = await ApplyFilterForGetAll(
                await GetDbSetAsync(),
                projectId: projectId,
                name: name,
                version: version,
                languageCode: languageCode,
                fileName: fileName,
                format: format,
                creationTimeMin: creationTimeMin,
                creationTimeMax: creationTimeMax,
                lastUpdatedTimeMin: lastUpdatedTimeMin,
                lastUpdatedTimeMax: lastUpdatedTimeMax,
                lastSignificantUpdateTimeMin: lastSignificantUpdateTimeMin,
                lastSignificantUpdateTimeMax: lastSignificantUpdateTimeMax,
                lastCachedTimeMin: lastCachedTimeMin, lastCachedTimeMax: lastCachedTimeMax);

            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<Document> FindAsync(Guid projectId, string name, string languageCode, string version,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).IncludeDetails(includeDetails)
                .FirstOrDefaultAsync(x =>
                    x.ProjectId == projectId && x.Name == name && x.LanguageCode == languageCode &&
                    x.Version == version,
                GetCancellationToken(cancellationToken));
        }

        public async Task<Document> FindAsync(Guid projectId, List<string> possibleNames, string languageCode, string version,
            bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).IncludeDetails(includeDetails)
                .FirstOrDefaultAsync(x =>
                    x.ProjectId == projectId && possibleNames.Contains(x.Name) &&
                    x.LanguageCode == languageCode && x.Version == version,
                GetCancellationToken(cancellationToken));
        }

        public virtual async Task DeleteAsync(Guid projectId, string name, string languageCode, string version, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await DeleteAsync(x =>
                x.ProjectId == projectId && x.Name == name && x.LanguageCode == languageCode &&
                x.Version == version, autoSave, cancellationToken: cancellationToken);
        }

        public virtual async Task<Document> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).Where(x => x.Id == id).SingleAsync(cancellationToken: GetCancellationToken(cancellationToken));
        }

        protected virtual async Task<IQueryable<DocumentWithoutContent>> ApplyFilterForGetAll(
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
                    d => d.LastCachedTime.Date <= lastCachedTimeMax.Value.Date)
                .Join( (await GetDbContextAsync()).Projects,
                    d => d.ProjectId,
                    p => p.Id,
                    (d, p) => new { d, p })
                .Select(x => new DocumentWithoutContent
                {
                    Id = x.d.Id,
                    ProjectId = x.d.ProjectId,
                    ProjectName = x.p.Name,
                    Name = x.d.Name,
                    Version = x.d.Version,
                    LanguageCode = x.d.LanguageCode,
                    FileName = x.d.FileName,
                    Format = x.d.Format,
                    CreationTime = x.d.CreationTime,
                    LastUpdatedTime = x.d.LastUpdatedTime,
                    LastSignificantUpdateTime = x.d.LastSignificantUpdateTime,
                    LastCachedTime = x.d.LastCachedTime
                });
        }
    }
}
