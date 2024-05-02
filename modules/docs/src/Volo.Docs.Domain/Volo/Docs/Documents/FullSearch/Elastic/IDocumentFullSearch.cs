using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Docs.Documents.FullSearch.Elastic
{
    public interface IDocumentFullSearch
    {
        Task CreateIndexIfNeededAsync(CancellationToken cancellationToken = default);

        Task AddOrUpdateAsync(Document document, CancellationToken cancellationToken = default);

        Task AddOrUpdateManyAsync(IEnumerable<Document> documents, CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task DeleteAllAsync(CancellationToken cancellationToken = default);

        Task DeleteAllByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);

        Task<List<EsDocument>> SearchAsync(string context, Guid projectId, string languageCode,
            string version, int? skipCount = null, int? maxResultCount = null,
            CancellationToken cancellationToken = default);

        void ValidateElasticSearchEnabled();
    }
}
