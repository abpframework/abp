using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Docs.Documents
{
    public interface IDocumentAppService : IApplicationService
    {
        Task<DocumentWithDetailsDto> GetByNameAsync(string projectShortName, string documentName, string version);

        Task<DocumentWithDetailsDto> GetNavigationDocumentAsync(string projectShortName, string version);

        Task<List<string>> GetVersions(string projectShortName, string documentName);
    }
}