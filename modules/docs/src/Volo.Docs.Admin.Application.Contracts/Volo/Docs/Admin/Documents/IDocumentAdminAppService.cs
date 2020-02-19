using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Docs.Admin.Documents
{
    public interface IDocumentAdminAppService : IApplicationService
    {
        Task PullAllAsync(PullAllDocumentInput input);

        Task PullAsync(PullDocumentInput input);
    }
}