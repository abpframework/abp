using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Docs.Documents
{
    public interface IDocumentAppService : IApplicationService
    {
        Task<DocumentWithDetailsDto> GetAsync(GetDocumentInput input);

        Task<DocumentWithDetailsDto> GetDefaultAsync(GetDefaultDocumentInput input);

        Task<DocumentWithDetailsDto> GetNavigationAsync(GetNavigationDocumentInput input);

        Task<DocumentResourceDto> GetResourceAsync(GetDocumentResourceInput input);
    }
}