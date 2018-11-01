using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public interface IDocumentStore : IDomainService
    {
        Task<Document> Find(
            Project project,
            string documentName,
            string version
        );

        Task<List<VersionInfoDto>> GetVersions(
            Project project
        );
    }
}