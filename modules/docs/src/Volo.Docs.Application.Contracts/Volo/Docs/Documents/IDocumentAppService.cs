using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public interface IDocumentAppService : IApplicationService
    {
        Task<DocumentWithDetailsDto> GetByNameAsync(string projectShortName, string documentName, string version,
            bool normalize);

        Task<NavigationWithDetailsDto> GetNavigationDocumentAsync(string projectShortName, string version,
            bool normalize);

        Task<List<string>> GetVersions(string projectShortName, string defaultDocumentName,
            Dictionary<string, object> projectExtraProperties,
            string documentStoreType, string documentName);

        Task<DocumentWithDetailsDto> GetDocument(ProjectDto project, string documentName, string version, bool normalize);
    }
}