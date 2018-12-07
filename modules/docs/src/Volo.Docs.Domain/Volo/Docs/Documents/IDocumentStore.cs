using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public interface IDocumentStore : IDomainService
    {
        Task<Document> GetDocument(Project project, string documentName, string version);

        Task<List<VersionInfo>> GetVersions(Project project);

        Task<DocumentResource> GetResource(Project project, string resourceName, string version);
    }
}