using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Docs.Admin.Documents
{
    public interface IDocumentAdminAppService : IApplicationService
    {
        Task ClearCacheAsync(ClearCacheInput input);

        Task PullAllAsync(PullAllDocumentInput input);

        Task PullAsync(PullDocumentInput input);
    }
}
