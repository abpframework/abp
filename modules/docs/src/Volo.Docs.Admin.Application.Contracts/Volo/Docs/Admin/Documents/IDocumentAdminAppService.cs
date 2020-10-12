using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Docs.Admin.Documents
{
    public interface IDocumentAdminAppService : IApplicationService
    {
        Task ClearCacheAsync(ClearCacheInput input);

        Task PullAllAsync(PullAllDocumentInput input);

        Task PullAsync(PullDocumentInput input);

        Task<PagedResultDto<DocumentDto>> GetAllAsync(GetAllInput input);

        Task RemoveFromCacheAsync(Guid documentId);

        Task ReindexAsync(Guid documentId);
    }
}
