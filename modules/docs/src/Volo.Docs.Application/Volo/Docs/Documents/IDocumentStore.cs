using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public interface IDocumentStore
    {
        Task<Document> FindDocumentByNameAsync(Project project, string documentName, string version);

        Task<List<string>> GetVersions(Project project, string documentName);
    }
}