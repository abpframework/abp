using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Docs.Documents
{
    public interface IDocumentAppService : IApplicationService
    {
        Task<DocumentWithDetailsDto> GetAsync(GetDocumentInput input);

        Task<DocumentWithDetailsDto> GetDefaultAsync(GetDefaultDocumentInput input);

        Task<NavigationNode> GetNavigationAsync(GetNavigationDocumentInput input);

        Task<DocumentParametersDto> GetParametersAsync(GetParametersDocumentInput input);

        Task<DocumentResourceDto> GetResourceAsync(GetDocumentResourceInput input);

        Task<List<DocumentSearchOutput>> SearchAsync(DocumentSearchInput input);
        
        Task<bool> FullSearchEnabledAsync();

        Task<List<string>> GetUrlsAsync(string prefix);
    }
}