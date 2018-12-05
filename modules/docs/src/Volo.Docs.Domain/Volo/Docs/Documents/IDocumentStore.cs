using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public interface IDocumentStore : IDomainService
    {
        Task<Document> FindDocument(Project project, string documentName, string version);

        Task<List<VersionInfo>> GetVersions(Project project);
    }
}