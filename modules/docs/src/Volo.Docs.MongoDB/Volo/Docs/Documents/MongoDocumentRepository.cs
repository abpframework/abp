using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using Volo.Docs.MongoDB;


namespace Volo.Docs.Documents
{
    public class MongoDocumentRepository : MongoDbRepository<IDocsMongoDbContext, Document, Guid>, IDocumentRepository
    {
        public MongoDocumentRepository(IMongoDbContextProvider<IDocsMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<List<DocumentWithoutDetails>> GetListWithoutDetailsByProjectId(Guid projectId, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
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
            return await (await GetMongoQueryableAsync(cancellationToken))
                .Select(x=> new DocumentInfo {
                    ProjectId = x.ProjectId,
                    Version = x.Version,
                    LanguageCode = x.LanguageCode,
                    Format = x.Format
                })
                .Distinct()
                .OrderByDescending(x=>x.Version)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Document>> GetListByProjectId(Guid projectId, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken)).Where(d => d.ProjectId == projectId).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Document>> GetUniqueDocumentsByProjectIdPagedAsync(Guid projectId, int skipCount, int maxResultCount,
            CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
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
            return await (await GetMongoQueryableAsync(cancellationToken)).Where(d => d.ProjectId == projectId)
                .GroupBy(x => new { x.Name, x.LanguageCode, x.Version })
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task UpdateProjectLastCachedTimeAsync(Guid projectId, DateTime cachedTime,
            CancellationToken cancellationToken = default)
        {
            var collection = await GetCollectionAsync(cancellationToken);
            await collection.UpdateManyAsync(
                Builders<Document>.Filter.Eq(x => x.ProjectId, projectId),
                Builders<Document>.Update.Set(x => x.LastCachedTime, cachedTime),
                cancellationToken: GetCancellationToken(cancellationToken)
            );
        }

        public virtual async Task<Document> FindAsync(Guid projectId, string name, string languageCode, string version,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken)).FirstOrDefaultAsync(x => x.ProjectId == projectId &&
                                                                                                    x.Name == name &&
                                                                                                    x.LanguageCode == languageCode &&
                                                                                                    x.Version == version, GetCancellationToken(cancellationToken));
        }
        
        public virtual async Task<Document> FindAsync(Guid projectId, List<string> possibleNames, string languageCode, string version,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken)).FirstOrDefaultAsync(x => x.ProjectId == projectId &&
                possibleNames.Contains(x.Name) &&
                x.LanguageCode == languageCode &&
                x.Version == version, GetCancellationToken(cancellationToken));
        }

        public virtual async Task DeleteAsync(Guid projectId, string name, string languageCode, string version, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await DeleteAsync(x =>
                x.ProjectId == projectId && x.Name == name && x.LanguageCode == languageCode &&
                x.Version == version, autoSave, cancellationToken: cancellationToken);
        }

        public virtual async Task<List<Document>> GetListAsync(Guid? projectId, string version, string name, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .WhereIf(version != null, x => x.Version == version)
                .WhereIf(name != null, x => x.Name == name)
                .WhereIf(projectId.HasValue, x => x.ProjectId == projectId)
                .As<IMongoQueryable<Document>>()
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

            return await
                (await ApplyFilterForGetAll(
                        await GetMongoQueryableAsync(cancellationToken),
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
                        lastCachedTimeMin: lastCachedTimeMin, lastCachedTimeMax: lastCachedTimeMax))
                    .OrderBy(string.IsNullOrWhiteSpace(sorting) ? "name asc" : sorting).As<IMongoQueryable<DocumentWithoutContent>>()
                    .PageBy<DocumentWithoutContent, IMongoQueryable<DocumentWithoutContent>>(skipCount, maxResultCount)
                    .ToListAsync(GetCancellationToken(cancellationToken));
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


            return await
                (await ApplyFilterForGetAll(
                        await GetMongoQueryableAsync(cancellationToken),
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
                        lastCachedTimeMin: lastCachedTimeMin, lastCachedTimeMax: lastCachedTimeMax))
                    .OrderBy(string.IsNullOrWhiteSpace(sorting) ? "name asc" : sorting).As<IMongoQueryable<Document>>()
                    .PageBy<Document, IMongoQueryable<Document>>(skipCount, maxResultCount)
                    .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<Document> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken)).Where(x => x.Id == id).SingleAsync(GetCancellationToken(cancellationToken));
        }
        
        protected virtual async Task<IMongoQueryable<DocumentWithoutContent>> ApplyFilterForGetAll(
            IMongoQueryable<Document> query,
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
            if (projectId.HasValue)
            {
                query = query.Where(d => d.ProjectId == projectId.Value);
            }

            if (name != null)
            {
                query = query.Where(d => d.Name != null && d.Name.Contains(name));
            }

            if (version != null)
            {
                query = query.Where(d => d.Version != null && d.Version == version);
            }

            if (languageCode != null)
            {
                query = query.Where(d => d.LanguageCode != null && d.LanguageCode == languageCode);
            }

            if (fileName != null)
            {
                query = query.Where(d => d.FileName != null && d.FileName.Contains(fileName));
            }
            
            if (format != null)
            {
                query = query.Where(d => d.Format != null && d.Format == format);
            }

            if (creationTimeMin.HasValue)
            {
                query = query.Where(d => d.CreationTime.Date >= creationTimeMin.Value.Date);
            }

            if (creationTimeMax.HasValue)
            {
                query = query.Where(d => d.CreationTime.Date <= creationTimeMax.Value.Date);
            }

            if (lastUpdatedTimeMin.HasValue)
            {
                query = query.Where(d => d.LastUpdatedTime.Date >= lastUpdatedTimeMin.Value.Date);
            }

            if (lastUpdatedTimeMax.HasValue)
            {
                query = query.Where(d => d.LastUpdatedTime.Date <= lastUpdatedTimeMax.Value.Date);
            }

            if (lastSignificantUpdateTimeMin.HasValue)
            {
                query = query.Where(d => d.LastSignificantUpdateTime != null && d.LastSignificantUpdateTime.Value.Date >= lastSignificantUpdateTimeMin.Value.Date);
            }

            if (lastSignificantUpdateTimeMax.HasValue)
            {
                query = query.Where(d => d.LastSignificantUpdateTime != null && d.LastSignificantUpdateTime.Value.Date <= lastSignificantUpdateTimeMax.Value.Date);
            }

            if (lastCachedTimeMin.HasValue)
            {
                query = query.Where(d => d.LastCachedTime.Date >= lastCachedTimeMin.Value.Date);
            }

            if (lastCachedTimeMax.HasValue)
            {
                query = query.Where(d => d.LastCachedTime.Date <= lastCachedTimeMax.Value.Date);
            }
            var join = query.Join(
                (await GetDbContextAsync(cancellationToken)).Projects,
                d => d.ProjectId,
                p => p.Id,
                (d, p) => new { Document = d, Project = p });
            return join.Select(x => new DocumentWithoutContent
            {
                Id = x.Document.Id,
                ProjectId = x.Document.ProjectId,
                ProjectName = x.Project.Name,
                Name = x.Document.Name,
                Version = x.Document.Version,
                LanguageCode = x.Document.LanguageCode,
                FileName = x.Document.FileName,
                Format = x.Document.Format,
                CreationTime = x.Document.CreationTime,
                LastUpdatedTime = x.Document.LastUpdatedTime,
                LastSignificantUpdateTime = x.Document.LastSignificantUpdateTime,
                LastCachedTime = x.Document.LastCachedTime
            });
        }
    }
}
