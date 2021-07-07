using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Docs.Documents
{
    public interface IDocumentRepository : IBasicRepository<Document>
    {
        Task<List<DocumentWithoutDetails>> GetListWithoutDetailsByProjectId(Guid projectId, CancellationToken cancellationToken = default);

        Task<List<Document>> GetListByProjectId(Guid projectId, CancellationToken cancellationToken = default);

        Task<Document> FindAsync(Guid projectId,
            string name,
            string languageCode,
            string version,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid projectId,
            string name,
            string languageCode,
            string version,
            CancellationToken cancellationToken = default);

        Task<List<DocumentWithoutContent>> GetAllAsync(
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
            CancellationToken cancellationToken = default);

        Task<long> GetAllCountAsync(
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
            CancellationToken cancellationToken = default);

        Task<Document> GetAsync(Guid id, CancellationToken cancellationToken = default);
    }
}