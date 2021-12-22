using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Comments;

public interface ICommentAdminAppService : IApplicationService
{
    Task<PagedResultDto<CommentWithAuthorDto>> GetListAsync(CommentGetListInput input);

    Task<CommentWithAuthorDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);
}
