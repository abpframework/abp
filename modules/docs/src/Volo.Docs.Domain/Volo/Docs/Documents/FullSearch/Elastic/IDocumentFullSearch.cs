using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents.FullSearch.Elastic
{
    public interface IDocumentFullSearch
    {
        Task CreateIndexIfNeededAsync(CancellationToken cancellationToken = default);

        Task AddOrUpdateAsync(Document document, CancellationToken cancellationToken = default);

        Task DeleteAsync(Document document, CancellationToken cancellationToken = default);

        Task DeleteAllAsync(CancellationToken cancellationToken = default);

        Task DeleteAllByProjectAsync(Project project, CancellationToken cancellationToken = default);

        Task ReindexDocumentAsync(Document document, CancellationToken cancellationToken = default);

        Task ReindexProjectAsync(Project project, CancellationToken cancellationToken = default);

        Task ReindexAllAsync(CancellationToken cancellationToken = default);

        Task<List<EsDocument>> SearchAsync(string context, Project project, string languageCode,
            string version, int? skipCount = null, int? maxResultCount = null,
            CancellationToken cancellationToken = default);
    }
}
