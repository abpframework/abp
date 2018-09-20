using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public interface IDocumentStore : IDomainService
    {
        Task<Document> FindDocumentByNameAsync(Project project, string documentName, string version);

        Task<List<string>> GetVersions(Project project, string documentName);
    }
}