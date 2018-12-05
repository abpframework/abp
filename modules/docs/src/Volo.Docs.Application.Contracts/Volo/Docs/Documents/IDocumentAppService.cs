using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Docs.Documents
{
    public interface IDocumentAppService : IApplicationService
    {
        Task<DocumentWithDetailsDto> GetAsync(GetDocumentInput input);

        Task<DocumentWithDetailsDto> GetDefaultAsync(GetDefaultDocumentInput input);

        Task<NavigationWithDetailsDto> GetNavigationDocumentAsync(GetNavigationDocumentInput input);
    }
}