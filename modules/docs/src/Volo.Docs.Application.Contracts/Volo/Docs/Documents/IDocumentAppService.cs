using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Docs.Documents
{
    public interface IDocumentAppService : IApplicationService
    {
        //TODO: Create input DTOs for methods and add validation annotations.

        Task<DocumentWithDetailsDto> GetByNameAsync(
            string projectShortName, 
            string documentName, 
            string version,
            bool normalize);

        Task<DocumentWithDetailsDto> GetDefaultAsync(
            string projectShortName,
            string version,
            bool normalize);

        Task<NavigationWithDetailsDto> GetNavigationDocumentAsync(
            string projectShortName, 
            string version,
            bool normalize);

        Task<List<VersionInfoDto>> GetVersions(
            string projectShortName
        );
    }
}