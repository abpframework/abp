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

        public async Task<List<Document>> GetListByProjectId(Guid projectId, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken)).Where(d => d.ProjectId == projectId).ToListAsync(cancellationToken);
        }

        public async Task<Document> FindAsync(Guid projectId, string name, string languageCode, string version,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken)).FirstOrDefaultAsync(x => x.ProjectId == projectId &&
                                                                                                    x.Name == name &&
                                                                                                    x.LanguageCode == languageCode &&
                                                                                                    x.Version == version, cancellationToken);
        }

        public async Task DeleteAsync(Guid projectId, string name, string languageCode, string version,
            CancellationToken cancellationToken = default)
        {
            await DeleteAsync(x =>
                x.ProjectId == projectId && x.Name == name && x.LanguageCode == languageCode &&
                x.Version == version, cancellationToken: cancellationToken);
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

            return await
                ApplyFilterForGetAll(
                        await GetMongoQueryableAsync(cancellationToken),
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
                        lastCachedTimeMax: lastCachedTimeMax)
                    .OrderBy(string.IsNullOrWhiteSpace(sorting) ? "name asc" : sorting).As<IMongoQueryable<Document>>()
                    .PageBy<Document, IMongoQueryable<Document>>(skipCount, maxResultCount)
                    .ToListAsync(GetCancellationToken(cancellationToken));
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


            return await
                ApplyFilterForGetAll(
                        await GetMongoQueryableAsync(cancellationToken),
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
                        lastCachedTimeMax: lastCachedTimeMax)
                    .OrderBy(string.IsNullOrWhiteSpace(sorting) ? "name asc" : sorting).As<IMongoQueryable<Document>>()
                    .PageBy<Document, IMongoQueryable<Document>>(skipCount, maxResultCount)
                    .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<Document> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken)).Where(x => x.Id == id).SingleAsync(cancellationToken);
        }

        protected virtual IMongoQueryable<Document> ApplyFilterForGetAll(
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

            return query;
        }
    }
}
