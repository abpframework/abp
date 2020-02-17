using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Docs.Documents
{
    public interface IDocumentRepository : IBasicRepository<Document>
    {
        Task<Document> FindAsync(Guid projectId, string name, string languageCode, string version,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid projectId, string name, string languageCode, string version,
            CancellationToken cancellationToken = default);
    }
}